namespace TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;

public sealed class RateLimitOptions
{
    public int MaxRequests { get; set; } = 1;
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(5);
}