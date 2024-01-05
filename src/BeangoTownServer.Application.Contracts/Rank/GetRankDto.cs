using Volo.Abp.Application.Dtos;

namespace BeangoTownServer.Rank;

public class GetRankDto : PagedResultRequestDto
{
    public string CaAddress { get; set; }
}