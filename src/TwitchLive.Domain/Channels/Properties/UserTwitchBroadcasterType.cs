namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchBroadcasterType(string Value)
{
    public static implicit operator string(UserTwitchBroadcasterType userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator UserTwitchBroadcasterType(string value) => new UserTwitchBroadcasterType(value);
}
