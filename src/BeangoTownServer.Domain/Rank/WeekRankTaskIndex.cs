using AElf.Indexing.Elasticsearch;
using BeangoTownServer.Entities;
using Nest;

namespace BeangoTownServer.Rank;

public class WeekRankTaskIndex : BeangoTownEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string SeasonId { get; set; }

    public int? Week { get; set; }

    public bool IsFinished { get; set; }

    public DateTime TriggerTime { get; set; }
}