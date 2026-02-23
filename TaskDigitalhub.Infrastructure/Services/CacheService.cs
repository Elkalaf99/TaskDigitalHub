using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _defaultExpiration;

    public CacheService(IMemoryCache cache, IConfiguration configuration)
    {
        _cache = cache;
        var minutes = int.Parse(configuration["Caching:DefaultExpirationMinutes"] ?? "5");
        _defaultExpiration = TimeSpan.FromMinutes(minutes);
    }

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        if (_cache.TryGetValue(key, out T? cached))
            return cached;

        var value = await factory();
        if (value != null)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration ?? _defaultExpiration);
            _cache.Set(key, value, options);
        }
        return value;
    }

    public void Remove(string key) => _cache.Remove(key);

    public void RemoveByPrefix(string prefix)
    {
        // IMemoryCache doesn't support key enumeration - use specific keys in handlers
        // For project/task invalidation we call Remove with exact keys
    }
}
