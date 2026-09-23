using TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;

namespace TwitchLive.Infrastructure.TwitchManagerSettings;

public class TwitchOptions
{
    public const string SectionName = "Twitch";
    public string Username { get; set; } = string.Empty;
    public string OAuthToken { get; set; } = string.Empty;
    public string BotId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string AccessToken { get; set; }= string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public List<string> PriorityChannels { get; set; } = new List<string>();
}
