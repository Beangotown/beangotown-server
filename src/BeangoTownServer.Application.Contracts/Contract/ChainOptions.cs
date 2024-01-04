using System.Collections.Generic;

namespace BeangoTownServer.Contract;

public class ChainOptions
{
    public Dictionary<string, ChainInfo> ChainInfos { get; set; }
}