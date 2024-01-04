using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace BeangoTownServer;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule))]
public class BeangoTownServerTestBaseModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => { options.IsJobExecutionEnabled = false; });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}