using System;
using System.Linq;
using AElf.ExceptionHandler;
using Microsoft.Extensions.Configuration;
using Serilog;
using Volo.Abp;

namespace BeangoTownServer;

public static class ConfigurationProvidersHelper
{
    /// <summary>
    /// The Method displays the enabled configuration providers in the order they were added,
    /// configuration providers that are added later have higher priority and override previous key settings.
    /// </summary>
    /// <param name="context"></param>
    [ExceptionHandler(typeof(Exception), TargetType = typeof(ExceptionHandlingService),
        MethodName = nameof(ExceptionHandlingService.HandleException), Message = "display configuration providers error.")]
    public static void DisplayConfigurationProviders(ApplicationInitializationContext context)
    {
        var configuration = context.GetConfiguration();
        var configurationRoot = (IConfigurationRoot)configuration;
        foreach (var provider in configurationRoot.Providers.ToList())
        {
            Log.Warning("ConfigurationProvider: {0}", provider.ToString());
        }
    }
}