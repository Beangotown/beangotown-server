using System.Threading.Tasks;

namespace BeangoTownServer.Trace;

public interface ITraceService
{
    public Task CreateAsync(GetUserActionDto getUserActionDto);
    
    public Task<StatResultDto> GetStatAsync(GetStatDto getStatDto);
}