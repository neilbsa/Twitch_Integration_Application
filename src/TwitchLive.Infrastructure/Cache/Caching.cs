using Microsoft.Extensions.Caching.Memory;

namespace TwitchLive.Infrastructure.Cache;

public sealed class Caching : ICaching
{
    private readonly IMemoryCache _memoryCache;
    public Caching(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set<T>(string key, T data, TimeSpan expiration)
    {
        var cachedData = new CachedData<T>(data, DateTimeOffset.UtcNow.Add(expiration));
        _memoryCache.Set(key, cachedData, expiration);
    }

    public T Get<T>(string key, out T data)
    {
        if (_memoryCache.TryGetValue(key, out CachedData<T>? cachedData))
        {
            if (cachedData?.Expiration > DateTimeOffset.UtcNow)
            {
                data = cachedData.Data;
                return data;
            }
            else
            {
                _memoryCache.Remove(key);
            }
        }
        data = default!;
        return data;
    }
}