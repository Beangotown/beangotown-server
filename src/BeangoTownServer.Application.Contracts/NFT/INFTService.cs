using System.Threading.Tasks;

namespace BeangoTownServer.NFT;

public interface INFTService
{
    public Task<BeanPassDto> ClaimBeanPassAsync(BeanPassInput input);


    public Task<BeanPassDto> IsBeanPassClaimableAsync(BeanPassInput input);
}