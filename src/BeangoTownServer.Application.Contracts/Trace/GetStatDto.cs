using System;
using Volo.Abp.Application.Dtos;

namespace BeangoTownServer.Trace;

public class GetStatDto
{
    public int Type { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}