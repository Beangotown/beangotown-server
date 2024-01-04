using System.Threading.Tasks;

namespace BeangoTownServer.Rank;

public interface IRankProvider
{
    public Task<WeekRankResultDto> GetWeekRankAsync(string caAddress, int skipCount, int maxResultCount);

    public Task<SeasonRecordDto> GetSeasonConfigAsync();

    public Task<SeasonDto> GetSeasonAsync(string seasonId);

    public Task<GameBlockHeightDto> GetLatestGameByBlockHeightAsync(long blockHeight);

    public Task<WeekRankRecordDto>
        GetWeekRankRecordsAsync(string seasonId, int week, int skipCount, int maxResultCount);

    public Task<SeasonRankRecordDto> GetSeasonRankRecordsAsync(string seasonId, int skipCount, int maxResultCount);

    public Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto getRankingHisDto);
}