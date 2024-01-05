using System.Threading.Tasks;
using BeangoTownServer.Rank;
using BeangoTownServer.Trace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BeangoTownServer;

[RemoteService]
[Route("api/app/trace/")]
public class TraceController : BeangoTownServerController
{
    private readonly ITraceService _traceService;
    private readonly ILogger<TraceController> _logger;

    public TraceController(ILogger<TraceController> logger, ITraceService traceService)
    {
        _logger = logger;
        _traceService = traceService;
    }
    [HttpPost]
    [Route("user-action")]
    public async Task CreateAsync(GetUserActionDto input)
    {
        await _traceService.CreateAsync(input);
    }

    [HttpGet]
    [Route("stat")]
    public async Task<StatResultDto> GetSeasonRankAsync(GetStatDto input)
    {
        return await _traceService.GetStatAsync(input);
    }
}