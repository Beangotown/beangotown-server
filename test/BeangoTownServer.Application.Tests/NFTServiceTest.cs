using System;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.MultiToken;
using BeangoTownServer.Cache;
using BeangoTownServer.Common;
using BeangoTownServer.Contract;
using BeangoTownServer.NFT;
using BeangoTownServer.Rank;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using StackExchange.Redis;
using Volo.Abp;
using Xunit;

namespace BeangoTownServer;

[Collection(BeangoTownServerTestConstants.CollectionDefinitionName)]
public class NFTServiceTest: BeangoTownServerApplicationTestBase
{
    private readonly INFTService _nftService;
    private readonly ICacheProvider _cacheProvider;
    private readonly IRankService _rankService;
    private const string _beanPassCacheKeyPrefix = "BeanPass_";
    private const string _beanPassUsingCacheKeyPrefix = "BeanPassUsing:";
    private const string CaAddress = "2D1yMpP8sskwRWqDmYKhvDGdtthv7oEWzmgGzffaNBPwaML6KE";
    private const string BeanPopPassCacheKeyPrefix = "BeanPassPop:";

    public NFTServiceTest()
    {
        _nftService = GetRequiredService<INFTService>();
        _cacheProvider = GetRequiredService<ICacheProvider>();
    }
    
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton<IRankProvider, MockGraphQLProvider>();
        services.AddSingleton(GetMockContractProvider());
        base.AfterAddApplication(services);
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

    [Fact]
    public async Task OtherBeanPassAsync_Test()
    {
        await _cacheProvider.SetAsync(_beanPassUsingCacheKeyPrefix + CaAddress,
            DateTime.UtcNow.ToTimestamp().ToString(),
            null);
        var beanPassInput = new BeanPassInput
        {
            CaAddress = CaAddress
        };
        var getBeanPassInput = new GetBeanPassInput
        {
            CaAddress = CaAddress,
            Symbol = "BEANPASS-2"
        };
        
        var result = await _nftService.UsingBeanPassAsync(getBeanPassInput);
        result.ShouldNotBeNull();
        getBeanPassInput.Symbol = "BB";
        var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () => await _nftService.UsingBeanPassAsync(getBeanPassInput));
        exception.Message.ShouldContain("currently not in your account");
        getBeanPassInput.Symbol = "BEANPASS-1";
        getBeanPassInput.CaAddress = "BBB";
        exception = await Assert.ThrowsAsync<UserFriendlyException>(async () => await _nftService.UsingBeanPassAsync(getBeanPassInput));
        exception.Message.ShouldContain("currently not in your account");
        var list = await _nftService.GetNftListAsync(beanPassInput);
        list.ShouldNotBeNull();
        var check = await _nftService.CheckBeanPassAsync(beanPassInput);
        check.ShouldBe(true);
    }
    
    private IContractProvider GetMockContractProvider()
    {
        var mockContractProvider = new Mock<IContractProvider>();
        mockContractProvider.Setup(o =>
                o.GetTokenInfo(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new TokenInfo { TokenName = "BEANPASS-2", ExternalInfo = new ExternalInfo()
            {
                Value =
                {
                    {
                        "__nft_image_url",
                        "https://i.seadn.io/gcs/files/0f5cdfaaf687de2ebb5834b129a5bef3.png?auto=format&w=3840"
                    }
                }
            } });

        return mockContractProvider.Object;
    }
}