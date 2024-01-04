using System.Threading.Tasks;
using BeangoTownServer.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BeangoTownServer;

[RemoteService]
[Route("api/app/chain/")]
public class ChainController : BeangoTownServerController
{
    private readonly ILogger<ChainController> _logger;
    private readonly IContractService _contractService;


    public ChainController(ILogger<ChainController> logger, IContractService contractService)
    {
        _logger = logger;
        _contractService = contractService;
    }

    [HttpGet]
    [Route("blockHeight")]
    public async Task<long> GetBlockHeightAsync()
    {
        return await _contractService.GetBlockHeightAsync();
    }
}