using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace BeangoTownServer.Monitor.Interceptor;

public class TimeConsumingInterceptor : AbpInterceptor, ITransientDependency
{
    public TimeConsumingInterceptor()
    {
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        // before method

        await invocation.ProceedAsync();

        // after method
    }
}