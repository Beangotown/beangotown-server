using System.Threading.Tasks;
using AElf;
using AElf.Client.Dto;
using AElf.Contracts.MultiToken;
using AElf.Types;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace BeangoTownServer.Contract;

public class ContractProvider : IContractProvider, ISingletonDependency
{
    private readonly ChainOptions _chainOptions;
    private readonly AElfClientFactory _factory;
    private readonly ILogger<ContractProvider> _logger;

    public ContractProvider(IOptionsSnapshot<ChainOptions> chainOptions,
        AElfClientFactory factory,
        ILogger<ContractProvider> logger)
    {
        _chainOptions = chainOptions.Value;
        _factory = factory;
        _logger = logger;
    }


    public async Task<GetBalanceOutput> GetBalanceAsync(string symbol, string address, string chainId)
    {
        if (!_chainOptions.ChainInfos.TryGetValue(chainId, out _)) return null;

        var getBalanceParam = new GetBalanceInput
        {
            Symbol = symbol,
            Owner = Address.FromBase58(address)
        };

        return await CallTransactionAsync<GetBalanceOutput>(chainId,
            _chainOptions.ChainInfos[chainId].TokenContractAddress, AElfConstants.GetBalance, getBalanceParam);
    }


    public async Task<long> GetBlockHeightAsync(string chainId)
    {
        var client = _factory.GetClient(chainId);
        return await client.GetBlockHeightAsync();
    }

    public async Task<SendTransactionOutput> SendTransferAsync(string symbol, string amount, string address,
        string chainId)
    {
        var transferParam = new TransferInput
        {
            Symbol = symbol,
            Amount = long.Parse(amount),
            To = Address.FromBase58(address)
        };

        return await SendTransactionAsync(chainId, _chainOptions.ChainInfos[chainId].TokenContractAddress,
            AElfConstants.Transfer, transferParam);
    }


    private async Task<T> CallTransactionAsync<T>(string chainId, string contractAddress, string methodName,
        IMessage param
    ) where T : class, IMessage<T>, new()
    {
        var key = _chainOptions.ChainInfos[chainId].PrivateKey;

        var client = _factory.GetClient(chainId);
        var transaction =
            await client.GenerateTransactionAsync(client.GetAddressFromPrivateKey(key), contractAddress, methodName,
                param);

        _logger.LogDebug("Call tx methodName is: {methodName} param is: {transaction}", methodName, transaction);

        var txWithSign = client.SignTransaction(key, transaction);
       
        var result = await client.ExecuteTransactionAsync(new ExecuteTransactionDto
        {
            RawTransaction = txWithSign.ToByteArray().ToHex()
        });

        var value = new T();
        value.MergeFrom(ByteArrayHelper.HexStringToByteArray(result));

        return value;
    }

    private async Task<SendTransactionOutput> SendTransactionAsync(string chainId, string contractAddress,
        string method, IMessage param)
    {
        var key = _chainOptions.ChainInfos[chainId].PrivateKey;

        var client = _factory.GetClient(chainId);

        var address = client.GetAddressFromPrivateKey(key);
        var transaction = await client.GenerateTransactionAsync(address, contractAddress, method, param);
        var txWithSign = client.SignTransaction(key, transaction);

        var rawTransaction = txWithSign.ToByteArray().ToHex();

        var result = await client.SendTransactionAsync(new SendTransactionInput
        {
            RawTransaction = rawTransaction
        });
        return result;
    }


    public async Task<TransactionResultDto> GetTransactionResultAsync(string chainId, string transactionId)
    {
        var client = _factory.GetClient(chainId);
        var result = await client.GetTransactionResultAsync(transactionId);
        return result;
    }
}