using AElf.ExceptionHandler.ABP;
using BeangoTownServer.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace BeangoTownServer
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AOPExceptionModule)

        //      typeof(AbpSettingManagementDomainSharedModule),
        //      typeof(AbpTenantManagementDomainSharedModule)
    )]
    public class BeangoTownServerDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BeangoTownServerGlobalFeatureConfigurator.Configure();
            BeangoTownServerModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BeangoTownServerDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BeangoTownServerResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/BeangoTownServer");

                options.DefaultResourceType = typeof(BeangoTownServerResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("BeangoTownServer", typeof(BeangoTownServerResource));
            });
        }
    }
}