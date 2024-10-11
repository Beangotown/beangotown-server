using System;
using System.Threading.Tasks;
using AElf.ExceptionHandler;
using BeangoTownServer.Common;
using Volo.Abp;

namespace BeangoTownServer;

public class ExceptionHandlingService
{
    public static async Task<FlowBehavior> HandleException(Exception ex)
    {
        return new FlowBehavior
        {
            ExceptionHandlingStrategy = ExceptionHandlingStrategy.Return,
            ReturnValue = true
        };
    }

    public static async Task<FlowBehavior> HandleGetCaHolderCreateTimeException(Exception ex)
    {
        throw new UserFriendlyException(BeangoTownConstants.SyncingMessage, BeangoTownConstants.SyncingCode);
    }

    public static async Task<FlowBehavior> HandleGetTokenBalanceException(Exception ex)
    {
        return new FlowBehavior
        {
            ExceptionHandlingStrategy = ExceptionHandlingStrategy.Return,
            ReturnValue = true
        };
    }
}