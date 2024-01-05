using AElf.Indexing.Elasticsearch;
using BeangoTownServer.Entities;
using Nest;

namespace BeangoTownServer.Trace;

public class UserActionIndex : BeangoTownEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string CaAddress { get; set; }
    [Keyword] public string CaHash { get; set; }
    [Keyword] public string ChainId { get; set; }
    public UserActionType ActionType { get; set; }
    public DateTime Timestamp { get; set; }
}

public enum UserActionType
{
    Register,
    Login
}