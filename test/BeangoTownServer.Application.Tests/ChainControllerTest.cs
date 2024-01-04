using System.Threading.Tasks;
using AElf.Client.Dto;
using BeangoTownServer.Contract;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit;

namespace BeangoTownServer;

[Collection(BeangoTownServerTestConstants.CollectionDefinitionName)]
public class ChainControllerTest : BeangoTownServerApplicationTestBase
{
    private readonly ChainController _chainController;

    public ChainControllerTest()
    {
        _chainController = GetRequiredService<ChainController>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton(GetMockContractProvider());
        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task GetBlockHeightTest()
    {
        var blockHeight = await _chainController.GetBlockHeightAsync();
        blockHeight.ShouldBe(20);
        blockHeight = await _chainController.GetBlockHeightAsync();
        blockHeight.ShouldBe(20);
    }

    private IContractProvider GetMockContractProvider()
    {
        var mockContractProvider = new Mock<IContractProvider>();
        mockContractProvider.Setup(o => o.GetBlockHeightAsync(It.IsAny<string>()))
            .ReturnsAsync(20
            );
        mockContractProvider.Setup(o =>
                o.SendTransferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new SendTransactionOutput { TransactionId = "" });

        return mockContractProvider.Object;
    }
}