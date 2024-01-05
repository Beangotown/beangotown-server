using System.Threading.Tasks;
using AElf.Types;

namespace BeangoTownServer.Contract;

public interface IContractService
{
    public Task<long> GetBlockHeightAsync();

    public Task<Hash> GetRandomHashAsync();
}