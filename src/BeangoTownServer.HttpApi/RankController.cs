using System.Threading.Tasks;
using BeangoTownServer.Rank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BeangoTownServer;

[RemoteService]
[Route("api/app/rank/")]
public class RankController : BeangoTownServerController
{
    private readonly IRankService _rankService;
    private readonly ILogger<RankController> _logger;

    public RankController(ILogger<RankController> logger, IRankService rankService)
    {
        _logger = logger;
        _rankService = rankService;
    }
    [HttpGet]
    [Route("week-rank")]
    public async Task<WeekRankResultDto> GetWeekRankAsync(GetRankDto input)
    {
        return await _rankService.GetWeekRankAsync(input);
    }

    [HttpGet]
    [Route("season-rank")]
    public async Task<SeasonRankResultDto> GetSeasonRankAsync(GetRankDto input)
    {
        return await _rankService.GetSeasonRankAsync(input);
    }

    [HttpGet]
    [Route("season-list")]
    public async Task<SeasonResultDto> GetRankingSeasonListAsync()
    {
        return await _rankService.GetSeasonConfigAsync();
    }

    [HttpGet]
    [Route("ranking-history")]
    public async Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto input)
    {
        return await _rankService.GetRankingHistoryAsync(input);
    }
}