﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BeangoTownServer.Common;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Volo.Abp.DistributedLocking;

namespace BeangoTownServer;

public abstract class
    BeangoTownServerApplicationTestBase : BeangoTownServerTestBase<BeangoTownServerApplicationTestModule>
{
   
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton(GetMockAbpDistributedLockAlwaysSuccess());
        services.AddSingleton(GetMockIGraphQLHelper());

    }

    protected IAbpDistributedLock GetMockAbpDistributedLockAlwaysSuccess()
    {
        var mockLockProvider = new Mock<IAbpDistributedLock>();
        mockLockProvider
            .Setup(x => x.TryAcquireAsync(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
            .Returns<string, TimeSpan, CancellationToken>((name, timeSpan, cancellationToken) =>
                Task.FromResult<IAbpDistributedLockHandle>(new LocalAbpDistributedLockHandle(new SemaphoreSlim(0))));
        return mockLockProvider.Object;
    }
    
    private IGraphQLHelper GetMockIGraphQLHelper()
    {
        var mockHelper = new Mock<IGraphQLHelper>();
        return mockHelper.Object;
    }

}