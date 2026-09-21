namespace TwitchLive.Domain.Followers.Properties;

public record FromLogin(string Value)
{
    public static implicit operator string(FromLogin userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator FromLogin(string value) => new FromLogin(value);
}
