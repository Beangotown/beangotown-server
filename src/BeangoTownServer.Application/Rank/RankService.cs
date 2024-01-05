using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Indexing.Elasticsearch;
using BeangoTown.Indexer.Plugin;
using BeangoTownServer.Common;
using BeangoTownServer.Contract;
using BeangoTownServer.Trace;
using Microsoft.Extensions.Options;
using Nest;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.ObjectMapping;

namespace BeangoTownServer.Rank;

[RemoteService(false), DisableAuditing]
public class RankService : BeangoTownServerAppService, IRankService
{
    private readonly INESTRepository<UserWeekRankIndex, string> _userRankWeekRepository;
    private readonly INESTRepository<UserSeasonRankIndex, string> _userSeasonWeekRepository;
    private readonly INESTRepository<RankSeasonConfigIndex, string> _rankSeasonRepository;
    private readonly INESTRepository<UserActionIndex, string> _userActionRepository;
    private readonly IRankProvider _rankProvider;
    private readonly IObjectMapper _objectMapper;
    private readonly ChainOptions _chainOptions;
    
    private const int QueryOnceLimit = 1000;
    private const string DateFormat = "yyyy-MM-dd";
    private const string StartTime = "00:00:00";

    public RankService(INESTRepository<UserWeekRankIndex, string> userRankWeekRepository,
        INESTRepository<UserSeasonRankIndex, string> userSeasonWeekRepository,
        INESTRepository<RankSeasonConfigIndex, string> rankSeasonRepository,
        INESTRepository<UserActionIndex, string> userActionRepository,
        IRankProvider rankProvider,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ChainOptions> chainOptions)
    {
        _userRankWeekRepository = userRankWeekRepository;
        _userSeasonWeekRepository = userSeasonWeekRepository;
        _rankSeasonRepository = rankSeasonRepository;
        _userActionRepository = userActionRepository;
        _objectMapper = objectMapper;
        _rankProvider = rankProvider;
        _chainOptions = chainOptions.Value;
    }


    public async Task<WeekRankResultDto> GetWeekRankAsync(GetRankDto getRankDto)
    {
        return await _rankProvider.GetWeekRankAsync(getRankDto.CaAddress, getRankDto.SkipCount,
            getRankDto.MaxResultCount);
    }

    public async Task<SeasonResultDto> GetSeasonConfigAsync()
    {
        var result = await _rankSeasonRepository.GetSortListAsync(
            sortFunc: s => s.Descending(a => Convert.ToInt64(a.Id))
        );

        return new SeasonResultDto
        {
            Season = _objectMapper.Map<List<RankSeasonConfigIndex>, List<SeasonDto>>(result.Item2)
        };
    }

    public async Task<SeasonRankResultDto> GetSeasonRankAsync(GetRankDto getRankDt)
    {
        var rankResultDto = new SeasonRankResultDto();
        var seasonIndex = await GetRankSeasonConfigIndexAsync();
        SeasonWeekHelper.GetSeasonStatusAndRefreshTime(seasonIndex, DateTime.Now, out var status, out var refreshTime);
        rankResultDto.SeasonName = seasonIndex?.Name;
        rankResultDto.Status = status;
        rankResultDto.RefreshTime = refreshTime;
        if (seasonIndex == null || getRankDt.SkipCount >= seasonIndex.PlayerSeasonShowCount) return rankResultDto;

        var mustQuery = new List<Func<QueryContainerDescriptor<UserSeasonRankIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.SeasonId).Value(seasonIndex.Id)));

        QueryContainer Filter(QueryContainerDescriptor<UserSeasonRankIndex> f)
        {
            return f.Bool(b => b.Must(mustQuery));
        }

        var result = await _userSeasonWeekRepository.GetSortListAsync(Filter, null,
            s => s.Descending(a => a.SumScore)
            , Math.Min(getRankDt.MaxResultCount, seasonIndex.PlayerSeasonShowCount - getRankDt.SkipCount),
            getRankDt.SkipCount);
        var rank = getRankDt.SkipCount;
        var rankDtos = new List<RankDto>();
        foreach (var item in result.Item2)
        {
            var rankDto = _objectMapper.Map<UserSeasonRankIndex, RankDto>(item);
            rankDto.Rank = ++rank;
            rankDtos.Add(rankDto);
            if (rankDto.CaAddress.Equals(getRankDt.CaAddress)) rankResultDto.SelfRank = rankDto;
        }

        rankResultDto.RankingList = rankDtos;

        var id = IdGenerateHelper.GenerateId(seasonIndex.Id, AddressHelper.ToShortAddress(getRankDt.CaAddress));
        if (rankResultDto.SelfRank == null)
        {
            var userSeasonRankIndex = await _userSeasonWeekRepository.GetAsync(id);
            rankResultDto.SelfRank = ConvertSeasonRankDto(getRankDt.CaAddress, userSeasonRankIndex);
        }

        return rankResultDto;
    }


    public async Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto getRankingHisDto)
    {
        if (string.IsNullOrEmpty(getRankingHisDto.CaAddress) || string.IsNullOrEmpty(getRankingHisDto.SeasonId))
            return new RankingHisResultDto();

        var id = IdGenerateHelper.GenerateId(getRankingHisDto.SeasonId,
            AddressHelper.ToShortAddress(getRankingHisDto.CaAddress));
        var userSeasonRankIndex = await _userSeasonWeekRepository.GetAsync(id);
        var seasonRankDto = ConvertSeasonRankDto(getRankingHisDto.CaAddress, userSeasonRankIndex);
        
        var rankingHisResultDto = await _rankProvider.GetRankingHistoryAsync(getRankingHisDto);


        var mustQuery = new List<Func<QueryContainerDescriptor<UserWeekRankIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.SeasonId).Value(getRankingHisDto.SeasonId)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.CaAddress).Value(getRankingHisDto.CaAddress)));

        QueryContainer Filter(QueryContainerDescriptor<UserWeekRankIndex> f)
        {
            return f.Bool(b => b.Must(mustQuery));
        }

        var result = await _userRankWeekRepository.GetSortListAsync(Filter, null,
            s => s.Ascending(a => a.Week)
        );
        var weekRankDtos = new List<WeekRankDto>();
        foreach (var userWeekRankIndex in result.Item2)
        {
            var userWeekRank = _objectMapper.Map<UserWeekRankIndex, WeekRankDto>(userWeekRankIndex);
            weekRankDtos.Add(userWeekRank);
        }

        if (!rankingHisResultDto.Weeks.IsNullOrEmpty())
            foreach (var weekRankDto in rankingHisResultDto.Weeks)
            {
                var weekRankExists = weekRankDtos.Exists(weekRank =>
                    weekRankDto.Week == weekRank.Week );
                if (!weekRankExists) weekRankDtos.Add(weekRankDto);
            }

        return new RankingHisResultDto
        {
            Weeks = weekRankDtos,
            Season = seasonRankDto
        };
    }

    public async Task SyncRankDataAsync()
    {
        var configList = await _rankProvider.GetSeasonConfigAsync();
        if (configList == null || configList.GetRankingSeasonList == null || configList.GetRankingSeasonList.Season == null) return;
        foreach (var config in configList.GetRankingSeasonList.Season)
        {
            var seasonId = config.Id;

            var skipCount = 0;
            while (true)
            {
                var seasonRankRecords = await _rankProvider.GetSeasonRankRecordsAsync(seasonId, skipCount, QueryOnceLimit);
                if(seasonRankRecords == null || seasonRankRecords.GetSeasonRankRecords == null || seasonRankRecords.GetSeasonRankRecords.RankingList == null) break;
                var rankCount = seasonRankRecords.GetSeasonRankRecords.RankingList?.Count ?? 0;
                if (rankCount == 0) break;
                skipCount += QueryOnceLimit;
                
                var seasonList = _objectMapper.Map<List<RankDto>, List<UserSeasonRankIndex>>(seasonRankRecords.GetSeasonRankRecords.RankingList);
                foreach (var item in seasonList)
                {
                    item.Id = IdGenerateHelper.GenerateId(seasonId, AddressHelper.ToShortAddress(item.CaAddress));
                    item.SeasonId = seasonId;
                }
                await _userSeasonWeekRepository.BulkAddOrUpdateAsync(seasonList);
            }

            var week = config.WeekInfos?.Count ?? 0;
            if (week == 0) continue;

            for (var i = 1; i <= week; i++)
            {
                skipCount = 0;
                var rank = 0;
                while (true)
                {
                    var weekRankRecords =
                        await _rankProvider.GetWeekRankRecordsAsync(seasonId, i, skipCount, QueryOnceLimit);
                    if (weekRankRecords == null || weekRankRecords.GetWeekRankRecords == null ||
                        weekRankRecords.GetWeekRankRecords.RankingList == null) break;
                    var rankCount = weekRankRecords.GetWeekRankRecords.RankingList?.Count ?? 0;
                    if (rankCount == 0) break;
                    skipCount += QueryOnceLimit;
                    var weekList =
                        _objectMapper.Map<List<RankDto>, List<UserWeekRankIndex>>(weekRankRecords.GetWeekRankRecords
                            .RankingList);
                    foreach (var item in weekList)
                    {
                        rank++;
                        if (rank <= config.PlayerWeekRankCount) item.Rank = rank;
                        item.Id = IdGenerateHelper.GenerateId(seasonId, i,
                            AddressHelper.ToShortAddress(item.CaAddress));
                        item.SeasonId = seasonId;
                        item.Week = i;
                        item.UpdateTime = DateTime.UtcNow;
                    }

                    await _userRankWeekRepository.BulkAddOrUpdateAsync(weekList);
                }
            }
        }

        var seasonConfigList =
            _objectMapper.Map<List<SeasonDto>, List<RankSeasonConfigIndex>>(configList.GetRankingSeasonList.Season);
        await _rankSeasonRepository.BulkAddOrUpdateAsync(seasonConfigList);
    }

    public async Task SyncGameDataAsync()
    {
        var chainId = GetDefaultChainId();
        var userActionList = new List<UserActionIndex>();
        var goRecords = await _rankProvider.GetGoRecordsAsync();
        foreach (var record in goRecords)
        {
            var userActionIndex = new UserActionIndex();
            userActionIndex.CaAddress = AddressHelper.ToShortAddress(record.CaAddress);
            userActionIndex.ChainId = chainId;
            userActionIndex.Timestamp = record.TriggerTime;
            userActionIndex.Id = $"{userActionIndex.CaAddress}_{userActionIndex.ChainId}_{DateTimeHelper.ToUnixTimeMilliseconds(userActionIndex.Timestamp)}";
            userActionIndex.ActionType = UserActionType.Register;
            userActionList.Add(userActionIndex);
            if (userActionList.Count == QueryOnceLimit)
            {
                await _userActionRepository.BulkAddOrUpdateAsync(userActionList);
                userActionList.Clear();
            }
        }

        if (userActionList.Count > 0 && userActionList.Count < QueryOnceLimit)
        {
            await _userActionRepository.BulkAddOrUpdateAsync(userActionList);
            userActionList.Clear();
        }

        var startDate = DateTimeHelper.DatetimeToString(DateTime.UtcNow.AddDays(-Convert.ToInt32(DateTime.UtcNow.DayOfWeek) - 6), DateFormat);
        
        var dto = new GetGameHistoryDto();
        dto.BeginTime = DateTimeHelper.ParseDateTimeByStr($"{startDate} {StartTime}").AddHours(-8);
        dto.EndTime = DateTime.UtcNow.AddHours(1);
        dto.SkipCount = 0;
        dto.MaxResultCount = QueryOnceLimit;
        var historyRecords = new GameHisResultDto();
        do
        {
            historyRecords = await _rankProvider.GetGameHistoryListAsync(dto);
            if (historyRecords == null || historyRecords.GameList.IsNullOrEmpty()) break;
            
            foreach (var gameDto in historyRecords.GameList)
            {
                if(gameDto.BingoTransactionInfo == null || goRecords.Exists(r => r.CaAddress == gameDto.CaAddress && r.TriggerTime == gameDto.BingoTransactionInfo.TriggerTime)) continue;
                var userActionIndex = new UserActionIndex();
                userActionIndex.CaAddress = AddressHelper.ToShortAddress(gameDto.CaAddress);
                userActionIndex.ChainId = chainId;
                userActionIndex.Timestamp = gameDto.BingoTransactionInfo.TriggerTime;
                userActionIndex.Id = $"{userActionIndex.CaAddress}_{userActionIndex.ChainId}_{DateTimeHelper.ToUnixTimeMilliseconds(userActionIndex.Timestamp)}";
                userActionIndex.ActionType = UserActionType.Login;
                userActionList.Add(userActionIndex);
            }
            await _userActionRepository.BulkAddOrUpdateAsync(userActionList);
            userActionList.Clear();
            dto.SkipCount += QueryOnceLimit;
        } while (historyRecords.GameList.Count >= QueryOnceLimit);
    }

    private RankDto ConvertSeasonRankDto(string caAddress,
        UserSeasonRankIndex userSeasonRankIndex)
    {
        if (userSeasonRankIndex == null)
            return new RankDto
            {
                CaAddress = caAddress,
                Score = 0,
                Rank = BeangoTownConstants.UserDefaultRank
            };

        return _objectMapper.Map<UserSeasonRankIndex, RankDto>(userSeasonRankIndex);
    }

    private async Task<RankSeasonConfigIndex?> GetRankSeasonConfigIndexAsync()
    {
        var now = DateTime.UtcNow;
        var mustQuery = new List<Func<QueryContainerDescriptor<RankSeasonConfigIndex>, QueryContainer>>();
        mustQuery.Add(q => q.DateRange(i => i.Field(f => f.RankBeginTime).LessThanOrEquals(now)));
        mustQuery.Add(q => q.DateRange(i => i.Field(f => f.ShowEndTime).GreaterThanOrEquals(now)));

        QueryContainer Filter(QueryContainerDescriptor<RankSeasonConfigIndex> f)
        {
            return f.Bool(b => b.Must(mustQuery));
        }

        var rankSeason = await _rankSeasonRepository.GetSortListAsync(Filter, null,
            s => s.Descending(a => Convert.ToInt64(a.Id))
            , 1
        );
        if (rankSeason.Item2.Count == 0) return null;

        return rankSeason.Item2[0];
    }
    
    private string GetDefaultChainId()
    {
        return _chainOptions.ChainInfos.Keys.First();
    }
}