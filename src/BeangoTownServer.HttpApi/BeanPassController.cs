using System.Collections.Generic;
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
    public async Task<BeanPassDto> IsBeanPassClaimableAsync(BeanPassInput input)
    {
        return await _nftService.IsBeanPassClaimableAsync(input);
    }

    [HttpPost]
    [Route("claim")]
    public async Task<BeanPassDto> ClaimBeanPassAsync(BeanPassInput input)
    {
        return await _nftService.ClaimBeanPassAsync(input);
    }
    
    [HttpGet]
    [Route("nft-list")]
    public async Task<List<BeanPassResultDto>> GetNftListAsync(BeanPassInput input)
    {
        return await _nftService.GetNftListAsync(input);
    }
    
    [HttpPost]
    [Route("using")]
    public async Task<BeanPassResultDto> UsingBeanPassAsync(GetBeanPassInput input)
    {
        return await _nftService.UsingBeanPassAsync(input);
    }
    
    [HttpPost]
    [Route("popup")]
    public async Task<bool> PopupBeanPassAsync(BeanPassInput input)
    {
         return await _nftService.PopupBeanPassAsync(input);
    }
    
    [HttpGet]
    [Route("check")]
    public async Task<bool> CheckBeanPassAsync(BeanPassInput input)
    {
        return await _nftService.CheckBeanPassAsync(input);
    }
}