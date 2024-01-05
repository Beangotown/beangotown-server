using AElf.Indexing.Elasticsearch;
using BeangoTownServer.Entities;
using Nest;

namespace BeangoTownServer.Rank;

public class UserSeasonRankIndex : BeangoTownEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string SeasonId { get; set; }

    [Keyword] public string CaAddress { get; set; }

    public long SumScore { get; set; }

    public int Rank { get; set; }
}