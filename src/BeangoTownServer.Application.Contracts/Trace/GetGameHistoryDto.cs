using System;
using Volo.Abp.Application.Dtos;

namespace BeangoTownServer.Trace;

public class GetGameHistoryDto : PagedResultRequestDto
{
    public string? CaAddress { get; set; }
    public DateTime BeginTime { get; set; }
    public DateTime EndTime { get; set; }
}