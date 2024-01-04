using System.Threading.Tasks;
using BeangoTownServer.NFT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BeangoTownServer;

[RemoteService]
[Route("api/app/bean-pass/")]
public class BeanPassController : BeangoTownServerController
{
    private readonly ILogger<BeanPassController> _logger;
    private readonly INFTService _nftService;

    public BeanPassController(ILogger<BeanPassController> logger, INFTService nftService)
    {
        _logger = logger;
        _nftService = nftService;
    }

    [HttpGet]
    [Route("claimable")]
    public async Task<BeanPassDto> IsBeanPassClaimable(BeanPassInput input)
    {
        return await _nftService.IsBeanPassClaimableAsync(input);
    }

    [HttpPost]
    [Route("claim")]
    public async Task<BeanPassDto> ClaimBeanPass(BeanPassInput input)
    {
        return await _nftService.ClaimBeanPassAsync(input);
    }
}