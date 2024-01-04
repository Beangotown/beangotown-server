using BeangoTownServer.Rank;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace BeangoTownServer.Worker;

public class BatchWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IRankService _rankService;
    private readonly ILogger<RankSyncWorker> _logger;
    private readonly WorkerOptions _workerOptions;
    private readonly IAbpDistributedLock _distributedLock;

    public BatchWorker(AbpAsyncTimer timer, 
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
        if (!_workerOptions.BatchStart) return;
        await using var handle =
            await _distributedLock.TryAcquireAsync(name: WorkerOptions.BatchLockKeyPrefix);
        if (handle == null)
        {
            Logger.LogInformation("batch, do not get lock, keys already exits.");
            _workerOptions.BatchStart = false;
            return;
        }

        _workerOptions.BatchStart = false;
        _logger.LogInformation("batch worker start");
        await _rankService.SyncRankDataAsync();
        _logger.LogInformation("batch worker end");
    }
}