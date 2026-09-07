namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchId(string Value)
{
    public static implicit operator string(UserTwitchId userTwitchId) => userTwitchId.Value;
    public static implicit operator UserTwitchId(string value) => new UserTwitchId(value);
}
