using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;

namespace BeangoTownServer;

[DependsOn(
    typeof(AbpDistributedLockingModule),
    typeof(BeangoTownServerDomainModule),
    typeof(BeangoTownServerApplicationContractsModule)
)]
public class BeangoTownServerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeangoTownServerApplicationModule>(); });
        context.Services.AddHttpClient();
    }
}