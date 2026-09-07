using TwitchLive.Infrastructure.TwitchOption;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;


namespace TwitchLive.Infrastructure.TwitchOption;

public sealed class TwitchOptions
{
    public string Username { get; set; } = string.Empty;
    public string OAuthToken { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public RateLimitOptions TwitchRateLimitOptions { get; set; } = new RateLimitOptions();
    public List<string> PriorityChannels { get; set; } = new List<string>();
}
