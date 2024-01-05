using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BeangoTownServer.Rank;

public class WeekRankResultGraphDto
{
    public WeekRankResultDto GetWeekRank { get; set; }
}

public class WeekRankRecordDto
{
    public WeekRankResultDto GetWeekRankRecords { get; set; }
}


public class WeekRankResultDto
{
    public int Status { get; set; }
    public DateTime? RefreshTime { get; set; }
    [CanBeNull] public List<RankDto> RankingList { get; set; }
    [CanBeNull] public RankDto SelfRank { get; set; }
}