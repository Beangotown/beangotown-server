using System.Threading.Tasks;

namespace BeangoTownServer.Rank;

public interface IRankService
{
    public Task<WeekRankResultDto> GetWeekRankAsync(GetRankDto getRankDto);

    public Task<SeasonRankResultDto> GetSeasonRankAsync(GetRankDto getRankDt);

    public Task<SeasonResultDto> GetSeasonConfigAsync();
    public Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto getRankingHisDto);

    public Task SyncRankDataAsync();
}