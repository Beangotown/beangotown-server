using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace BeangoTownServer.Common;

public class HttpClientProvider : IHttpClientProvider, ISingletonDependency
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetAsync([CanBeNull] string token, string url)
    {
        var client = _httpClientFactory.CreateClient();
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetStringAsync(url);
        return response;
    }
}