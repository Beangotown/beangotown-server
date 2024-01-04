using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.AutoMapper;

namespace BeangoTownServer;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule))]
public class BeangoTownServerHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeangoTownServerHttpApiModule>(); });
    }
}