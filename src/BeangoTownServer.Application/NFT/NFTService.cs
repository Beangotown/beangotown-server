using System;
using System.Linq;
using System.Threading.Tasks;
using BeangoTownServer.Cache;
using BeangoTownServer.Common;
using BeangoTownServer.Contract;
using BeangoTownServer.Portkey;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;

namespace BeangoTownServer.NFT;

public class NFTService : BeangoTownServerAppService, INFTService
{
    private readonly string _beanPassCacheKeyPrefix = "BeanPass_";
    private readonly string _beanPassLimitCacheKeyPrefix = "BeanPassLimit_";
    private readonly ICacheProvider _cacheProvider;
    private readonly IContractProvider _contractProvider;
    private readonly ILogger<NFTService> _logger;
    private readonly UserActivityOptions _userActivityOptions;
    private readonly IPortkeyProvider _portkeyProvider;
    private readonly ChainOptions _chainOptions;

    public NFTService(IPortkeyProvider portkeyProvider, ICacheProvider cacheProvider,
        IContractProvider contractProvider,
        IOptionsSnapshot<UserActivityOptions> userActivityOptions,
        IOptionsSnapshot<ChainOptions> chainOptions)
    {
        _portkeyProvider = portkeyProvider;
        _cacheProvider = cacheProvider;
        _contractProvider = contractProvider;
        _userActivityOptions = userActivityOptions.Value;
        _chainOptions = chainOptions.Value;
    }

    public async Task<BeanPassDto> ClaimBeanPassAsync(BeanPassInput input)
    {
        var beanPass = await IsBeanPassClaimableAsync(input);
        if (!beanPass.Claimable) return beanPass;
        if (beanPass.Reason.Equals(ClaimBeanPassStatus.NewUser.ToString()))
        {
            var value = await _cacheProvider.IncreaseAsync(_beanPassLimitCacheKeyPrefix + DateTime.UtcNow.Day, 1,
                TimeSpan.FromDays(2));
            if (value > _userActivityOptions.ClaimCountPerDay)
                return new BeanPassDto
                {
                    Claimable = false,
                    Reason = ClaimBeanPassStatus.Claimed.ToString()
                };
        }

        var sendTransactionOutput = await _contractProvider.SendTransferAsync(_userActivityOptions.BeanPass, "1",
            input.CaAddress,
            GetDefaultChainId()
        );
        
        await _cacheProvider.SetAsync(_beanPassCacheKeyPrefix + input.CaAddress,
            DateTime.UtcNow.ToTimestamp().ToString(),
            null);

        return new BeanPassDto
        {
            Claimable = true,
            TransactionId = sendTransactionOutput.TransactionId
        };
    }

    public async Task<BeanPassDto> IsBeanPassClaimableAsync(BeanPassInput input)
    {
        var beanPassValue = await _cacheProvider.GetAsync(_beanPassCacheKeyPrefix + input.CaAddress);
        if (!beanPassValue.IsNull)
            return new BeanPassDto
            {
                Claimable = false,
                Reason = ClaimBeanPassStatus.DoubleClaim.ToString()
            };
        var balance = await _portkeyProvider.GetTokenBalanceAsync(input);
        if (balance >= _userActivityOptions.NeedElfAmount)
            return new BeanPassDto
            {
                Claimable = true,
                Reason = ClaimBeanPassStatus.ElfAmountEnough.ToString()
            };
        var time = await _portkeyProvider.GetCaHolderCreateTimeAsync(input);
        if (time == 0) throw new UserFriendlyException("Syncing on-chain account info");
        var begin = DateTimeHelper.ParseDateTimeByStr(_userActivityOptions.BeginTime);
        var end = DateTimeHelper.ParseDateTimeByStr(_userActivityOptions.EndTime);
        var createTime = DateTimeHelper.FromUnixTimeSeconds(time);
        if (begin.CompareTo(createTime) == -1 && createTime.CompareTo(end) == -1)
        {
            var value = await _cacheProvider.GetAsync(_beanPassLimitCacheKeyPrefix + DateTime.UtcNow.Day);
            if (int.TryParse(value, out var count) && count > _userActivityOptions.ClaimCountPerDay)
                return new BeanPassDto
                {
                    Claimable = false,
                    Reason = ClaimBeanPassStatus.Claimed.ToString()
                };
            return new BeanPassDto
            {
                Claimable = true,
                Reason = ClaimBeanPassStatus.NewUser.ToString()
            };
        }

        return new BeanPassDto
        {
            Claimable = false,
            Reason = ClaimBeanPassStatus.InsufficientElfAmount.ToString()
        };
    }

    private string GetDefaultChainId()
    {
        return _chainOptions.ChainInfos.Keys.First();
    }
}