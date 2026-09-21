namespace TwitchLive.Domain.Followers.Properties;

public record ToLogin(string Value)
{
    public static implicit operator string(ToLogin userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator ToLogin(string value) => new ToLogin(value);
}
