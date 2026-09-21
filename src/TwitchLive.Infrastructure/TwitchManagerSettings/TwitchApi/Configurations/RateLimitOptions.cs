namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;

public sealed class RateLimitOptions
{
    public int MaxRequests { get; set; } = 500;
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(5);
}