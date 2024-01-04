using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BeangoTownServer.Cache;
using StackExchange.Redis;

namespace BeangoTownServer.Common;

public class MockCacheProvider : ICacheProvider
{
    private readonly ConcurrentDictionary<string, string> _localCache = new();

    public Task SetAsync(string key, string value, TimeSpan? expire)
    {
        _localCache[key] = value;
        return Task.CompletedTask;
    }

    public Task<RedisValue> GetAsync(string key)
    {
        _localCache.TryGetValue(key, out var val);
        return Task.FromResult(new RedisValue(val));
    }

    public Task<long> IncreaseAsync(string key, int increase, TimeSpan? expire)
    {
        _localCache.TryAdd(key, "0");
        var val = _localCache.AddOrUpdate(key, increase.ToString,
            (key, oldValue) => (Convert.ToInt64(oldValue) + increase).ToString());
        return Task.FromResult(Convert.ToInt64(val));
    }
}