using AElf.Indexing.Elasticsearch;
using BeangoTown.Indexer.Plugin;
using BeangoTownServer.Cache;
using BeangoTownServer.Common;
using BeangoTownServer.Rank;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;

namespace BeangoTownServer.Worker;

public class RankSyncWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly ILogger<RankSyncWorker> _logger;
    private readonly WorkerOptions _workerOptions;
    private readonly IAbpDistributedLock _distributedLock;
    private readonly ICacheProvider _cacheProvider;
    private readonly IRankProvider _rankProvider;
    private readonly INESTRepository<UserWeekRankIndex, string> _userRankWeekRepository;
    private readonly INESTRepository<UserSeasonRankIndex, string> _userSeasonWeekRepository;
    private readonly INESTRepository<RankSeasonConfigIndex, string> _rankSeasonRepository;
    private readonly IObjectMapper _objectMapper;
    private const int QueryOnceLimit = 1000;

    public RankSyncWorker(AbpAsyncTimer timer, 
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RankSyncWorker> logger,
        IOptionsSnapshot<WorkerOptions> workerOptions,
        ICacheProvider cacheProvider,
        IRankProvider rankProvider,
        IObjectMapper objectMapper,
        INESTRepository<UserWeekRankIndex, string> userRankWeekRepository,
        INESTRepository<UserSeasonRankIndex, string> userSeasonWeekRepository,
        INESTRepository<RankSeasonConfigIndex, string> rankSeasonRepository,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory)
    {
        _logger = logger;
        _workerOptions = workerOptions.Value;
        timer.Period = _workerOptions.RankTimePeriod;
        _distributedLock = distributedLock;
        _cacheProvider = cacheProvider;
        _rankProvider = rankProvider;
        _userRankWeekRepository = userRankWeekRepository;
        _userSeasonWeekRepository = userSeasonWeekRepository;
        _rankSeasonRepository = rankSeasonRepository;
        _objectMapper = objectMapper;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using var handle =
            await _distributedLock.TryAcquireAsync(name: WorkerOptions.RankLockKeyPrefix);
        if (handle == null)
        {
            Logger.LogInformation("rank sync, do not get lock, keys already exits.");
            return;
        }
        
        _logger.LogInformation("rank sync start");
        var blockHeightRedisValue = await _cacheProvider.GetAsync(WorkerOptions.RankBlockHeightPrefix);
        long blockHeight = 0;
        long.TryParse(blockHeightRedisValue, out blockHeight);

        var latestGame = await _rankProvider.GetLatestGameByBlockHeightAsync(blockHeight);
        if (latestGame.GameCount == 0) return;
        var seasonIndex = await SaveAndGetSeasonConfig(latestGame);
        if (seasonIndex == null) return;
        var now = DateTime.UtcNow;
        var rankWeekNum =
            SeasonWeekHelper.GetRankWeekNum(seasonIndex, now);
        var showWeekNum = SeasonWeekHelper.GetShowWeekNum(seasonIndex, now);
        var isFinished = showWeekNum > rankWeekNum;
        if (!isFinished) return;

        var weekNum = Math.Max(showWeekNum, rankWeekNum);
        var isTaskSaved = await SaveRankWeekTaskAsync(seasonIndex, weekNum, isFinished);

        if (isTaskSaved) await SaveRankInfoAsync(seasonIndex, weekNum, isFinished);

        var seasonTask =
            await _cacheProvider.GetAsync(WorkerOptions.RankWeekTaskPrefix + seasonIndex.Id);

        if (bool.TryParse(seasonTask, out var isTaskFinished) && !isTaskFinished)
        {
            await RefreshSeasonRankAsync(seasonIndex.Id, seasonIndex.PlayerSeasonRankCount);
            await _cacheProvider.SetAsync(WorkerOptions.RankWeekTaskPrefix + seasonIndex.Id, true.ToString(),
                TimeSpan.FromDays(_workerOptions.TaskExpireDays));
        }

        // if weekRanking is Finished,start seasonRanking 
        if (isTaskSaved && isFinished)
            await _cacheProvider.SetAsync(WorkerOptions.RankWeekTaskPrefix + seasonIndex.Id, false.ToString(),
                TimeSpan.FromDays(_workerOptions.TaskExpireDays));
        
        await _cacheProvider.SetAsync(WorkerOptions.RankBlockHeightPrefix, latestGame.BingoBlockHeight.ToString(),
            null);
    }

    private async Task<RankSeasonConfigIndex?> SaveAndGetSeasonConfig(GameBlockHeightDto latestGame)
    {
        RankSeasonConfigIndex? seasonIndex = null;
        if (string.IsNullOrEmpty(latestGame.SeasonId))
        {
            seasonIndex = await GetRankSeasonConfigIndexAsync();
        }
        else
        {
            var seasonAsync = await _rankProvider.GetSeasonAsync(latestGame.SeasonId);
            seasonIndex = _objectMapper.Map<SeasonDto, RankSeasonConfigIndex>(seasonAsync);
            await _rankSeasonRepository.AddOrUpdateAsync(seasonIndex);
        }

        return seasonIndex;
    }


    private async Task SaveRankInfoAsync(RankSeasonConfigIndex? seasonIndex, int weekNum, bool isFinished)
    {
        var skipCount = 0;
        var rank = 0;
        while (true)
        {
            var weekRankRecords =
                await _rankProvider.GetWeekRankRecordsAsync(seasonIndex.Id, weekNum, skipCount, QueryOnceLimit);
            if (weekRankRecords == null || weekRankRecords.GetWeekRankRecords == null ||
                weekRankRecords.GetWeekRankRecords.RankingList == null) break;
            var rankCount = weekRankRecords.GetWeekRankRecords.RankingList?.Count ?? 0;
            _logger.LogDebug("rankResult.Item2.Count {Count}", rankCount);
            if (rankCount == 0) break;
            skipCount += QueryOnceLimit;
            var weekList =
                _objectMapper.Map<List<RankDto>, List<UserWeekRankIndex>>(weekRankRecords.GetWeekRankRecords
                    .RankingList);
            var now = DateTime.UtcNow;
            foreach (var item in weekList)
            {
                if (rank < seasonIndex.PlayerWeekRankCount)
                {
                    item.Rank = ++rank;
                }
                item.Id = IdGenerateHelper.GenerateId(seasonIndex.Id, weekNum,
                    AddressHelper.ToShortAddress(item.CaAddress));
                item.SeasonId = seasonIndex.Id;
                item.Week = weekNum;
                item.UpdateTime = now;
            }

            await _userRankWeekRepository.BulkAddOrUpdateAsync(weekList);
            if (isFinished)
                foreach (var userWeek in weekList)
                    await SaveSeasonUserRankAsync(userWeek);
        }
       
    }

    private async Task SaveSeasonUserRankAsync(UserWeekRankIndex item)
    {
        if (item.Rank == BeangoTownConstants.UserDefaultRank) return;

        var rankSeasonUserId = IdGenerateHelper.GenerateId(item.SeasonId, AddressHelper.ToShortAddress(item.CaAddress));
        var rankSeasonUser =
            await _userSeasonWeekRepository.GetAsync(rankSeasonUserId);
        if (rankSeasonUser == null)
            rankSeasonUser = new UserSeasonRankIndex
            {
                Id = rankSeasonUserId,
                SeasonId = item.SeasonId,
                CaAddress = item.CaAddress,
                SumScore = 0,
                Rank = BeangoTownConstants.UserDefaultRank
            };

        rankSeasonUser.SumScore = Math.Max(rankSeasonUser.SumScore, item.SumScore);
        await _userSeasonWeekRepository.AddOrUpdateAsync(rankSeasonUser);
    }

    private async Task<bool> SaveRankWeekTaskAsync(RankSeasonConfigIndex? seasonIndex, int weekNum,
        bool isFinished)
    {
        var rankWeekTaskId = IdGenerateHelper.GenerateId(seasonIndex.Id, weekNum);
        var rankWeekTask = await _cacheProvider.GetAsync(WorkerOptions.RankWeekTaskPrefix + rankWeekTaskId);
        if (bool.TryParse(rankWeekTask, out var isTaskFinished) && isTaskFinished) return false;
        await _cacheProvider.SetAsync(WorkerOptions.RankWeekTaskPrefix + rankWeekTaskId, isFinished.ToString(),
            TimeSpan.FromDays(_workerOptions.TaskExpireDays));
        return true;
    }

    private async Task RefreshSeasonRankAsync(string seasonId, int rankCount)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<UserSeasonRankIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.SeasonId).Value(seasonId)));

        QueryContainer Filter(QueryContainerDescriptor<UserSeasonRankIndex> f)
        {
            return f.Bool(b => b.Must(mustQuery));
        }

        var needRankResult = await _userSeasonWeekRepository.GetListAsync(Filter, null, s => s.SumScore,
            SortOrder.Descending,
            skip: 0,
            limit: rankCount);
        mustQuery.Add(q => q.Range(i => i.Field(f => f.Rank).GreaterThan(BeangoTownConstants.UserDefaultRank)));

        QueryContainer RankFilter(QueryContainerDescriptor<UserSeasonRankIndex> f)
        {
            return f.Bool(b => b.Must(mustQuery));
        }

        var hasRankResult = await _userSeasonWeekRepository.GetListAsync(RankFilter);
        var userSeasonRankList = new List<UserSeasonRankIndex>();
        for (var i = 0; i < needRankResult.Item2.Count; i++)
        {
            var item = needRankResult.Item2[i];
            item.Rank = i + 1;
            userSeasonRankList.Add(item);
        }

        await _userSeasonWeekRepository.BulkAddOrUpdateAsync(userSeasonRankList);

        // del expired ranking
        var idList = needRankResult.Item2.Select(item => item.Id);
        var userSeasonRankIndices = hasRankResult.Item2.FindAll(item => !idList.Contains(item.Id));
        if (userSeasonRankIndices.IsNullOrEmpty()) return;
        foreach (var notRankUser in userSeasonRankIndices) notRankUser.Rank = BeangoTownConstants.UserDefaultRank;
        await _userSeasonWeekRepository.BulkAddOrUpdateAsync(userSeasonRankIndices);
    }

    private async Task<RankSeasonConfigIndex?> GetRankSeasonConfigIndexAsync()
    {
        var rankSeason = await _rankSeasonRepository.GetSortListAsync(null, null,
            s => s.Descending(a => Convert.ToInt64(a.Id))
            , 1
        );
        if (rankSeason.Item2.Count == 0) return null;

        return rankSeason.Item2[0];
    }
    
}