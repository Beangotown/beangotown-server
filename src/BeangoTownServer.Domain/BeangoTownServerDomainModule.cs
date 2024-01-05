using AElf.Indexing.Elasticsearch;
using BeangoTownServer.Rank;
using BeangoTownServer.Rank.Etos;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace BeangoTownServer
{
    [DependsOn(
        typeof(BeangoTownServerDomainSharedModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AElfIndexingElasticsearchModule)
    )]
    public class BeangoTownServerDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeangoTownServerDomainModule>(); });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.AutoEventSelectors.Add<RankSeasonConfigIndex>();
                options.EtoMappings.Add<RankSeasonConfigIndex, RankSeasonConfigEto>();
                
                options.AutoEventSelectors.Add<UserSeasonRankIndex>();
                options.EtoMappings.Add<UserSeasonRankIndex, UserSeasonRankEto>();
                
                options.AutoEventSelectors.Add<UserWeekRankIndex>();
                options.EtoMappings.Add<UserWeekRankIndex, UserWeekRankEto>();

                options.AutoEventSelectors.Add<WeekRankTaskIndex>();
                options.EtoMappings.Add<WeekRankTaskIndex, WeekRankTaskEto>();
            });
        }
    }
}
