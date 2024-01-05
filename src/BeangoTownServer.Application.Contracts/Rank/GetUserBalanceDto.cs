using System.Collections.Generic;

namespace BeangoTownServer.Rank;

public class GetUserBalanceDto
{
    public string ChainId { get; set; } 
    public string CaAddress { get; set; }
    public List<string> Symbols { get; set; }
}