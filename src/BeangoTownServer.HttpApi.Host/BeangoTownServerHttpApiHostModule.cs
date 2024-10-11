using System;
using System.Linq;
using AElf.Indexing.Elasticsearch.Options;
using AutoResponseWrapper;
using BeangoTownServer.Contract;
using BeangoTownServer.NFT;
using BeangoTownServer.Portkey;
using BeangoTownServer.Redis;
using BeangoTownServer.Worker;
using Medallion.Threading;
using Medallion.Threading.Redis;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace BeangoTownServer;

[DependsOn(
    typeof(BeangoTownServerHttpApiModule),
    typeof(BeangoTownServerRedisModule),
    typeof(BeangoTownServerApplicationModule),
    typeof(BeangoTownServerWorkerModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpDistributedLockingModule)
)]
public class BeangoTownServerHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        Configure<ChainOptions>(configuration.GetSection("Chains"));
        Configure<PortkeyOptions>(configuration.GetSection("Portkey"));
        Configure<UserActivityOptions>(configuration.GetSection("UserActivity"));
        Configure<HalloweenActivityOptions>(configuration.GetSection("HalloweenActivity"));
        Configure<WorkerOptions>(configuration.GetSection("Worker"));
        ConfigureConventionalControllers();
        ConfigureCache(context, configuration);
        ConfigureAuditing();
        ConfigureEsIndexCreation();
        ConfigureLocalization();
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        ConfigureDistributedLocking(context, configuration);
        ConfigureGraphQl(context, configuration);
        context.Services.AddAutoResponseWrapper();
    }

    private void ConfigureCache(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var multiplexer = ConnectionMultiplexer
            .Connect(configuration["Redis:Configuration"]);
        context.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "BeangoTownServer:"; });
    }
    
    private void ConfigureAuditing()
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = false;
        });
    }
    
    private void ConfigureEsIndexCreation()
    {
        Configure<IndexCreateOption>(x => { x.AddModule(typeof(BeangoTownServerDomainModule)); });
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(BeangoTownServerHttpApiModule).Assembly);
        });
    }


    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseHsts();

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();

        app.UseAbpRequestLocalization();
        app.UseAuthorization();

        //  if (env.IsDevelopment())
        //   {
        app.UseSwagger();
        app.UseAbpSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API"); });
        //  }

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
        
        ConfigurationProvidersHelper.DisplayConfigurationProviders(context);
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BeangoTownServer API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            }
        );
    }
    
    private void ConfigureDistributedLocking(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }

    private void ConfigureGraphQl(ServiceConfigurationContext context,
        IConfiguration configuration)
    {
        context.Services.AddSingleton(new GraphQLHttpClient(configuration["GraphQL:Configuration"],
            new NewtonsoftJsonSerializer()));
        context.Services.AddScoped<IGraphQLClient>(sp => sp.GetRequiredService<GraphQLHttpClient>());
    }
}