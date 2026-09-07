namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchDisplayName(string Value)
{
    public static implicit operator string(UserTwitchDisplayName userTwitchDisplayName) => userTwitchDisplayName.Value;
    public static implicit operator UserTwitchDisplayName(string value) => new UserTwitchDisplayName(value);
}
