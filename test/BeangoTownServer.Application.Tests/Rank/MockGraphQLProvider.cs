using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeangoTownServer.Trace;
using Volo.Abp.DependencyInjection;

namespace BeangoTownServer.Rank;

public class MockGraphQLProvider : IRankProvider, ISingletonDependency
{
    private const string CaAddress = "23GxsoW9TRpLqX1Z5tjrmcRMMSn5bhtLAf4HtPj8JX9BerqTqp";
    private const string UserCaAddress = "2D1yMpP8sskwRWqDmYKhvDGdtthv7oEWzmgGzffaNBPwaML6KE";
    private const string SeasonId = "1";
    private const string SeasonName = "Season-1";

    public async Task<WeekRankResultDto> GetWeekRankAsync(string caAddress, int skipCount, int maxResultCount)
    {
        var dto = new WeekRankResultDto();
        dto.RankingList = new List<RankDto>
        {
            new()
            {
                CaAddress = CaAddress,
                Score = 5,
                Rank = 1
            }
        };
        dto.SelfRank = new RankDto();
        return dto;
    }

    public async Task<SeasonRecordDto> GetSeasonConfigAsync()
    {
        var dto = new SeasonRecordDto();
        dto.GetRankingSeasonList = new SeasonResultDto();
        dto.GetRankingSeasonList.Season = new List<SeasonDto>();
        var seasonDto = new SeasonDto
        {
            Id = SeasonId,
            Name = SeasonName,
            PlayerWeekRankCount = 1,
            PlayerWeekShowCount = 1,
            PlayerSeasonRankCount = 1,
            PlayerSeasonShowCount = 1,
            RankBeginTime = DateTime.UtcNow.AddDays(-1),
            RankEndTime = DateTime.UtcNow,
            ShowBeginTime = DateTime.UtcNow,
            ShowEndTime = DateTime.UtcNow.AddDays(1)
        };
        seasonDto.WeekInfos = new List<WeekDto>
        {
            new()
            {
                RankBeginTime = DateTime.UtcNow.AddDays(-1),
                RankEndTime = DateTime.UtcNow,
                ShowBeginTime = DateTime.UtcNow,
                ShowEndTime = DateTime.UtcNow.AddDays(1)
            }
        };
        dto.GetRankingSeasonList.Season.Add(seasonDto);
        return dto;
    }

    public async Task<SeasonDto> GetSeasonAsync(string seasonId)
    {
        var dto = new SeasonDto
        {
            Id = seasonId,
            Name = SeasonName,
            PlayerWeekRankCount = 1,
            PlayerWeekShowCount = 1,
            PlayerSeasonRankCount = 1,
            PlayerSeasonShowCount = 1,
            RankBeginTime = DateTime.UtcNow.AddDays(-1),
            RankEndTime = DateTime.UtcNow,
            ShowBeginTime = DateTime.UtcNow,
            ShowEndTime = DateTime.UtcNow.AddDays(1),
            WeekInfos = new List<WeekDto>
            {
                new()
                {
                    RankBeginTime = DateTime.UtcNow.AddDays(-1),
                    RankEndTime = DateTime.UtcNow,
                    ShowBeginTime = DateTime.UtcNow,
                    ShowEndTime = DateTime.UtcNow.AddDays(1)
                }
            }
        };
        return dto;
    }

    public async Task<GameBlockHeightDto> GetLatestGameByBlockHeightAsync(long blockHeight)
    {
        return new GameBlockHeightDto
        {
            BingoBlockHeight = 0,
            LatestGameId = "1",
            SeasonId = SeasonId,
            GameCount = 1
        };
    }

    public async Task<WeekRankRecordDto> GetWeekRankRecordsAsync(string seasonId, int week, int skipCount,
        int maxResultCount)
    {
        var dto = new WeekRankRecordDto();
        if (skipCount > 100) return dto;
        dto.GetWeekRankRecords = new WeekRankResultDto();
        dto.GetWeekRankRecords.RankingList = new List<RankDto>();
        var rankDto = new RankDto
        {
            CaAddress = "BB",
            Score = 70,
            Rank = 1
        };
        dto.GetWeekRankRecords.RankingList.Add(rankDto);
        return dto;
    }

    public async Task<SeasonRankRecordDto> GetSeasonRankRecordsAsync(string seasonId, int skipCount, int maxResultCount)
    {
        var dto = new SeasonRankRecordDto();
        if (skipCount > 100) return dto;
        dto.GetSeasonRankRecords = new SeasonRankResultDto();
        dto.GetSeasonRankRecords.RankingList = new List<RankDto>();
        var rankDto = new RankDto
        {
            CaAddress = "AA",
            Score = 80,
            Rank = 1
        };
        dto.GetSeasonRankRecords.RankingList.Add(rankDto);
        return dto;
    }

    public async Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto getRankingHisDto)
    {
        var dto = new RankingHisResultDto
        {
            Season = new RankDto
            {
                CaAddress = CaAddress,
                Score = 7,
                Rank = 1
            },
            Weeks = new List<WeekRankDto>
            {
                new()
                {
                    Week = 1,
                    CaAddress = CaAddress,
                    Score = 7,
                    Rank = 1
                }
            }
        };
        return dto;
    }

    public async Task<List<GameRecordDto>> GetGoRecordsAsync()
    {
        return new List<GameRecordDto>
        {
            new()
            {
                Id = SeasonId,
                CaAddress = CaAddress,
                TriggerTime = DateTime.Now
            }
        };
    }

    public async Task<int> GetGoCountAsync(GetGoDto dto)
    {
        return dto.SkipCount > 100 ? 1 : 0;
    }
    
    public async Task<GameHisResultDto> GetGameHistoryListAsync(GetGameHistoryDto dto)
    {
        var result = new GameHisResultDto();
        result.GameList = new List<GameResultDto>()
        {
            new GameResultDto()
            {
                CaAddress = CaAddress,
                BingoTransactionInfo = new TransactionInfoDto()
                {
                    TriggerTime = DateTime.Now
                }
            }
        };
        return result;
    }

    public async Task<List<UserBalanceDto>> GetUserBalanceAsync(GetUserBalanceDto dto)
    {
        return new List<UserBalanceDto>()
        {
            new UserBalanceDto()
            {
                Symbol = "BEANPASS-2",
                Amount = dto.Symbols.Contains("BEANPASS-2") ? 1 : 0
            }
        };
    }
}