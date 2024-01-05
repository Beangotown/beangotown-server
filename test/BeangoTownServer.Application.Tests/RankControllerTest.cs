using System.Threading;
using System.Threading.Tasks;
using BeangoTownServer.Rank;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BeangoTownServer;
[Collection(BeangoTownServerTestConstants.CollectionDefinitionName)]
public class RankControllerTest : BeangoTownServerApplicationTestBase
{
    private readonly RankController _rankController;
    private readonly IRankService _rankService;
    private const string CaAddress = "23GxsoW9TRpLqX1Z5tjrmcRMMSn5bhtLAf4HtPj8JX9BerqTqp";

    public RankControllerTest()
    {
        _rankController = GetRequiredService<RankController>();
        _rankService = GetRequiredService<IRankService>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton<IRankProvider, MockGraphQLProvider>();
        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task GetWeekRankAsync()
    {
        var input = new GetRankDto
        {
            CaAddress = CaAddress
        };

        var weekRankAsync = await _rankController.GetWeekRankAsync(input);
        weekRankAsync.RankingList.ShouldNotBeNull();
        weekRankAsync.SelfRank.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetSeasonRankAsyncTest()
    {
        await RankSyncWorkTest();
        Thread.Sleep(2000);
        var input = new GetRankDto
        {
            CaAddress = CaAddress
        };
        var seasonRankAsync = await _rankController.GetSeasonRankAsync(input);
        seasonRankAsync.RankingList.ShouldNotBeNull();
        seasonRankAsync.SelfRank.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetRankingSeasonListAsyncTest()
    {
        await _rankService.SyncRankDataAsync();
        Thread.Sleep(2000);
        var seasonResultDto = await _rankController.GetSeasonConfigAsync();
        seasonResultDto.Season.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetRankingHistoryAsync()
    {
        var input = new GetRankingHisDto
        {
            SeasonId = "10",
            CaAddress = "21mEqQqL1L79QDcryCCbFPv9nYjj7SCefsBrXMMkajE7iFmgkD"
        };

        var rankHistory = await _rankController.GetRankingHistoryAsync(input);
        rankHistory.ShouldNotBeNull();
    }

    [Fact]
    public async Task RankSyncWorkTest()
    {
        await _rankService.SyncRankDataAsync();
    }

    [Fact]
    public async Task SyncGameDataAsync()
    {
        await _rankService.SyncGameDataAsync();
    }
}