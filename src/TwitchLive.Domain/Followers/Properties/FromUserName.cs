namespace TwitchLive.Domain.Followers.Properties;

public record FromUserName(string Value)
{
    public static implicit operator string(FromUserName userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator FromUserName(string value) => new FromUserName(value);
}
