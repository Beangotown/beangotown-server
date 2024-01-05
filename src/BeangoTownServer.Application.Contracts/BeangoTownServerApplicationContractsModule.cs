using BeangoTownServer.Monitor;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace BeangoTownServer;

[DependsOn(
    typeof(AbpObjectExtendingModule),
    typeof(BeangoTownServerMonitorModule)
)]
public class BeangoTownServerApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }
}