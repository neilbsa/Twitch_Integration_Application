namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchType(string Value)
{
    public static implicit operator string(UserTwitchType userTwitchType) => userTwitchType.Value;
    public static implicit operator UserTwitchType(string value) => new UserTwitchType(value);
}
