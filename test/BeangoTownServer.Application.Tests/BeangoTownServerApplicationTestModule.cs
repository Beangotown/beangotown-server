using System;
using System.Collections.Generic;
using BeangoTownServer.Cache;
using BeangoTownServer.Common;
using BeangoTownServer.Contract;
using BeangoTownServer.NFT;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace BeangoTownServer;

[DependsOn(
    typeof(BeangoTownServerApplicationModule),
    typeof(BeangoTownServerApplicationContractsModule),
    typeof(BeangoTownServerHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class BeangoTownServerApplicationTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<ICacheProvider, MockCacheProvider>();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeangoTownServerApplicationModule>(); });
        Configure<UserActivityOptions>(option =>
        {
            option.BeginTime = DateTimeHelper.DatetimeToString(DateTime.Now.AddDays(-1));
            option.EndTime = DateTimeHelper.DatetimeToString(DateTime.Now.AddDays(1));
            option.ClaimCountPerDay = 1;
            option.BeanPass = "BEANPASS-1";
            option.NeedElfAmount = 2;
        });
        Configure<HalloweenActivityOptions>(option =>
        {
            option.BeginTime = DateTimeHelper.DatetimeToString(DateTime.Now.AddDays(-1));
            option.EndTime = DateTimeHelper.DatetimeToString(DateTime.Now.AddDays(1));
            option.BeanPass = new List<string>
            {
                "BEANPASS-2", "BEANPASS-1"
            };
        });
        Configure<ChainOptions>(options =>
        {
            var infos = new Dictionary<string, ChainInfo>();
            infos.Add("TDVW", new ChainInfo
            {
                ChainId = "TDVW",
                BaseUrl = "",
                TokenContractAddress = "",
                PrivateKey = ""
            });
            options.ChainInfos = infos;
        });
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeangoTownServerHttpApiModule>(); });
        base.ConfigureServices(context);
    }
}