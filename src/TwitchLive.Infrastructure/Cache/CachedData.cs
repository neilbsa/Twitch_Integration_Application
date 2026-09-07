namespace TwitchLive.Infrastructure.Cache;

public sealed record CachedData<T>(T Data, DateTimeOffset Expiration);
