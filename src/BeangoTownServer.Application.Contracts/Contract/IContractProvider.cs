using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.MultiToken;

namespace BeangoTownServer.Contract;

public interface IContractProvider
{
    public Task<GetBalanceOutput> GetBalanceAsync(string symbol, string address, string chainId);
    public Task<SendTransactionOutput> SendTransferAsync(string symbol, string amount, string address, string chainId);

    public Task<long> GetBlockHeightAsync(string chainId);

}