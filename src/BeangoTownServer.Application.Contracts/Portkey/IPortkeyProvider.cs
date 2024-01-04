using System.Threading.Tasks;
using BeangoTownServer.NFT;

namespace BeangoTownServer.Portkey;

public interface IPortkeyProvider
{
    public Task<long> GetCaHolderCreateTimeAsync(BeanPassInput beanPassInput);
    public Task<long> GetTokenBalanceAsync(BeanPassInput beanPassInput);
}