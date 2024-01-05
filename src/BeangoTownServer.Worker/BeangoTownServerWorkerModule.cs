using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;

namespace BeangoTownServer.Worker
{
    [DependsOn(
        typeof(BeangoTownServerApplicationContractsModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpDistributedLockingModule)
    )]
    public class BeangoTownServerWorkerModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var backgroundWorkerManger = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
            backgroundWorkerManger.AddAsync(context.ServiceProvider.GetService<RankSyncWorker>());
            backgroundWorkerManger.AddAsync(context.ServiceProvider.GetService<BatchWorker>());
            backgroundWorkerManger.AddAsync(context.ServiceProvider.GetService<GameSyncWorker>());
        }
    }
}