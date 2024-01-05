using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BeangoTownServer.Monitor;

public class BeangoTownServerMonitorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IndicatorOptions>(context.Services.GetConfiguration().GetSection("Indicator"));
    }
}