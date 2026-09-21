namespace TwitchLive.Domain.Followers.Properties;

public record ToUserId(string Value)
{
    public static implicit operator string(ToUserId userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator ToUserId(string value) => new ToUserId(value);
}
