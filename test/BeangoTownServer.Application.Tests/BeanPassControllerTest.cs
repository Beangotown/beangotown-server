using System;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.MultiToken;
using BeangoTownServer.Common;
using BeangoTownServer.Contract;
using BeangoTownServer.NFT;
using BeangoTownServer.Portkey;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace BeangoTownServer;

[Collection(BeangoTownServerTestConstants.CollectionDefinitionName)]
public class BeanPassControllerTest : BeangoTownServerApplicationTestBase
{
    private readonly BeanPassController _beanPassController;
    private readonly UserActivityOptions _userActivityOptions;
    private readonly string _elfEnoughUserAddress = "3xPfEaMkQnUicXsUWCT1SgdqK8JUpXvcpZYjZ8FgF5PLdfw4w";
    private readonly string _newUserAddress1 = "21mEqQqL1L79QDcryCCbFPv9nYjj7SCefsBrXMMkajE7iFmgkD";
    private readonly string _newUserAddress2 = "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx";
    private readonly string _userAddress = "JZdkBE8WTm88TWpUkXk9ep7AHmHnkPFDeLExwfffc2RZnawGt";

    public BeanPassControllerTest()
    {
        _beanPassController = GetRequiredService<BeanPassController>();
        _userActivityOptions = GetRequiredService<IOptionsSnapshot<UserActivityOptions>>().Value;
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton(GetMockPortkeyProvider());
        services.AddSingleton(GetMockContractProvider());
        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task IsBeanPassClaimableTest()
    {
        var result = await _beanPassController.IsBeanPassClaimable(new BeanPassInput
        {
            CaAddress = _newUserAddress1
        });
        result.Claimable.ShouldBe(true);
        result.Reason.ShouldBe(ClaimBeanPassStatus.NewUser.ToString());

        result = await _beanPassController.IsBeanPassClaimable(new BeanPassInput
        {
            CaAddress = _elfEnoughUserAddress
        });
        result.Claimable.ShouldBe(true);
        result.Reason.ShouldBe(ClaimBeanPassStatus.ElfAmountEnough.ToString());

        result = await _beanPassController.ClaimBeanPass(new BeanPassInput
        {
            CaAddress = _newUserAddress1
        });
        result.Claimable.ShouldBe(true);
        result = await _beanPassController.ClaimBeanPass(new BeanPassInput
        {
            CaAddress = _newUserAddress1
        });
        result.Claimable.ShouldBe(false);
        result.Reason.ShouldBe(ClaimBeanPassStatus.DoubleClaim.ToString());
        result = await _beanPassController.ClaimBeanPass(new BeanPassInput
        {
            CaAddress = _newUserAddress2
        });
        result.Claimable.ShouldBe(false);
        result.Reason.ShouldBe(ClaimBeanPassStatus.Claimed.ToString());
        result = await _beanPassController.ClaimBeanPass(new BeanPassInput
        {
            CaAddress = _userAddress
        });
        result.Claimable.ShouldBe(false);
        result.Reason.ShouldBe(ClaimBeanPassStatus.InsufficientElfAmount.ToString());
        
    }

    private IContractProvider GetMockContractProvider()
    {
        var mockContractProvider = new Mock<IContractProvider>();
        mockContractProvider.Setup(o => o.GetBalanceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new GetBalanceOutput
                {
                    Balance = 80
                }
            );
        mockContractProvider.Setup(o =>
                o.SendTransferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new SendTransactionOutput { TransactionId = "" });

        return mockContractProvider.Object;
    }

    private IPortkeyProvider GetMockPortkeyProvider()
    {
        var mockPortkeyProvider = new Mock<IPortkeyProvider>();
        mockPortkeyProvider.Setup(o => o.GetTokenBalanceAsync(It.IsAny<BeanPassInput>()))
            .ReturnsAsync((BeanPassInput beanPassInput) =>
                beanPassInput.CaAddress.Equals(_elfEnoughUserAddress)
                    ? _userActivityOptions.NeedElfAmount + 1
                    : _userActivityOptions.NeedElfAmount - 1
            );
        mockPortkeyProvider.Setup(o =>
                o.GetCaHolderCreateTimeAsync(It.IsAny<BeanPassInput>()))
            .ReturnsAsync((BeanPassInput beanPassInput) =>
                beanPassInput.CaAddress.Equals(_newUserAddress1) || beanPassInput.CaAddress.Equals(_newUserAddress2)
                ? DateTimeHelper.ToUnixTimeMilliseconds(DateTime.Now) / 1000
                : DateTimeHelper.ToUnixTimeMilliseconds(DateTime.MinValue) / 1000
            );
        return mockPortkeyProvider.Object;
    }
}