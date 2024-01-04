#nullable enable
using System.Threading.Tasks;

namespace BeangoTownServer.Common;

public interface IHttpClientProvider
{
    Task<string> GetAsync(string? token, string url);
}