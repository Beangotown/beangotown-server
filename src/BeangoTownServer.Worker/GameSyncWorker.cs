using BeangoTownServer.Rank;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace BeangoTownServer.Worker;

public class GameSyncWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IRankService _rankService;
    private readonly ILogger<RankSyncWorker> _logger;
    private readonly WorkerOptions _workerOptions;
    private readonly IAbpDistributedLock _distributedLock;

    public GameSyncWorker(AbpAsyncTimer timer, 
        IServiceScopeFactory serviceScopeFactory,
        IRankService rankService,
        ILogger<RankSyncWorker> logger,
        IOptionsSnapshot<WorkerOptions> workerOptions,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory)
    {
        _rankService = rankService;
        _logger = logger;
        _workerOptions = workerOptions.Value;
        timer.Period = _workerOptions.RankTimePeriod;
        _distributedLock = distributedLock;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        if (!_workerOptions.GameSyncStart) return;
        await using var handle =
            await _distributedLock.TryAcquireAsync(name: WorkerOptions.GameSyncLockKeyPrefix);
        if (handle == null)
        {
            Logger.LogInformation("game sync, do not get lock, keys already exits.");
            _workerOptions.GameSyncStart = false;
            return;
        }

        _workerOptions.GameSyncStart = false;
        _logger.LogInformation("game sync worker start");
        await _rankService.SyncGameDataAsync();
        _logger.LogInformation("game sync worker end");
    }
}