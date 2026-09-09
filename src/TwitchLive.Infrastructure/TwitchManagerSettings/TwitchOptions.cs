using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;

namespace TwitchLive.Infrastructure.TwitchManagerSettings;

public class TwitchOptions
{
    public const string SectionName = "Twitch";
    public string Username { get; set; } = string.Empty;
    public string OAuthToken { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public RateLimitOptions TwitchRateLimitOptions { get; set; } = new RateLimitOptions();
    public List<string> PriorityChannels { get; set; } = new List<string>();
}
