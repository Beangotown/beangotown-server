using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AElf.Indexing.Elasticsearch;
using AElf.Indexing.Elasticsearch.Options;
using AElf.Indexing.Elasticsearch.Services;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace BeangoTownServer;

[DependsOn(
    typeof(BeangoTownServerTestBaseModule)
)]
public class BeangoTownServerDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IndexCreateOption>(x => { x.AddModule(typeof(BeangoTownServerDomainModule)); });

        // Do not modify this!!!
        context.Services.Configure<EsEndpointOption>(options =>
        {
            options.Uris = new List<string> { "http://127.0.0.1:9200" };
        });

        context.Services.Configure<IndexSettingOptions>(options =>
        {
            options.NumberOfReplicas = 1;
            options.NumberOfShards = 1;
            options.Refresh = Refresh.True;
            options.IndexPrefix = "beangotownservertest";
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var elasticIndexService = context.ServiceProvider.GetRequiredService<IElasticIndexService>();
        var modules = context.ServiceProvider.GetRequiredService<IOptionsSnapshot<IndexCreateOption>>().Value.Modules;

        modules.ForEach(m =>
        {
            var types = GetTypesAssignableFrom<IIndexBuild>(m.Assembly);
            foreach (var t in types)
                AsyncHelper.RunSync(async () =>
                    await elasticIndexService.DeleteIndexAsync("beangotownservertest." + t.Name.ToLower()));
        });
    }

    private List<Type> GetTypesAssignableFrom<T>(Assembly assembly)
    {
        var compareType = typeof(T);
        return assembly.DefinedTypes
            .Where(type => compareType.IsAssignableFrom(type) && !compareType.IsAssignableFrom(type.BaseType) &&
                           !type.IsAbstract && type.IsClass && compareType != type)
            .Cast<Type>().ToList();
    }
}