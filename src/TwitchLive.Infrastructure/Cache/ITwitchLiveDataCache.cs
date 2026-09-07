namespace TwitchLive.Infrastructure.Cache;

public interface ITwitchLiveDataCache
{
    T Get<T>(string key, out T data);
    void Set<T>(string key, T data, TimeSpan expiration);
}
