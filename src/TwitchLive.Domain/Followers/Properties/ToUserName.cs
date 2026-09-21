namespace TwitchLive.Domain.Followers.Properties;

public record ToUserName(string Value)
{
    public static implicit operator string(ToUserName userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator ToUserName(string value) => new ToUserName(value);
}