namespace BeangoTownServer.Contract;

public class ChainInfo
{
    public string ChainId { get; set; }
    public string BaseUrl { get; set; }
    public string TokenContractAddress { get; set; }
    public string ConsensusContract { get; set; }
    public string PrivateKey { get; set; }
}