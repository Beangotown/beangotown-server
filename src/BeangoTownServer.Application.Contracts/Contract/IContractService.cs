using System.Threading.Tasks;

namespace BeangoTownServer.Contract;

public interface IContractService
{
    public Task<long> GetBlockHeightAsync();
}