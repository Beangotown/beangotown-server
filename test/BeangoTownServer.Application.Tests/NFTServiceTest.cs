using System;
using System.Threading.Tasks;
using BeangoTownServer.Cache;
using BeangoTownServer.Common;
using BeangoTownServer.NFT;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace BeangoTownServer;

public class NFTServiceTest: BeangoTownServerApplicationTestBase
{
    private readonly INFTService _nftService;
    private readonly ICacheProvider _cacheProvider;
    private const string _beanPassCacheKeyPrefix = "BeanPass_";
    private const string CaAddress = "2D1yMpP8sskwRWqDmYKhvDGdtthv7oEWzmgGzffaNBPwaML6KE";
    private const string BeanPopPassCacheKeyPrefix = "BeanPassPop:";

    public NFTServiceTest()
    {
        _nftService = GetRequiredService<INFTService>();
        _cacheProvider = GetRequiredService<ICacheProvider>();
    }
    
    [Fact]
    public async Task PopupBeanPassAsync_Pop_Old_Test()
    {
        await _cacheProvider.SetAsync(_beanPassCacheKeyPrefix + CaAddress,
            DateTime.UtcNow.ToTimestamp().ToString(),
            null);
        var beanPassInput = new BeanPassInput
        {
            CaAddress = CaAddress
        };
        var popupBeanPassResult = await _nftService.PopupBeanPassAsync(beanPassInput);
        popupBeanPassResult.ShouldBe(false);
    }
    
    
    [Fact]
    public async Task PopupBeanPassAsync_NoPop_New_Test()
    {
        await _cacheProvider.SetAsync(_beanPassCacheKeyPrefix + CaAddress,
            DateTime.UtcNow.AddDays(20).ToTimestamp().ToString(),
            null);
        var beanPassInput = new BeanPassInput
        {
            CaAddress = CaAddress
        };
        var popupBeanPassResult = await _nftService.PopupBeanPassAsync(beanPassInput);
        popupBeanPassResult.ShouldBe(false);
    }
    
    
    
    [Fact]
    public async Task PopupBeanPassAsync_NoPop_Old_Test()
    {
        await _cacheProvider.SetAsync(_beanPassCacheKeyPrefix + CaAddress,
            DateTime.UtcNow.ToTimestamp().ToString(),
            null);
        var beanPassInput = new BeanPassInput
        {
            CaAddress = CaAddress
        };
        var popupBeanPassResult = await _nftService.PopupBeanPassAsync(beanPassInput);
        popupBeanPassResult.ShouldBe(false);
        
        var popupBeanPassResultSecond = await _nftService.PopupBeanPassAsync(beanPassInput);
        
        popupBeanPassResultSecond.ShouldBe(false);
        var redisValue = await _cacheProvider.GetAsync(BeanPopPassCacheKeyPrefix + CaAddress);
        redisValue.ShouldBe(RedisValue.Null);
    }
}