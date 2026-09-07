namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchOfflineImageUrl(string Value)
{
    public static implicit operator string(UserTwitchOfflineImageUrl userTwitchOfflineImageUrl) => userTwitchOfflineImageUrl.Value;
    public static implicit operator UserTwitchOfflineImageUrl(string value) => new UserTwitchOfflineImageUrl(value);
}
