namespace TwitchLive.Domain.Followers.Properties;

public record FromUserId(string Value)
{
    public static implicit operator string(FromUserId userTwitchBroadcasterType) => userTwitchBroadcasterType.Value;
    public static implicit operator FromUserId(string value) => new FromUserId(value);
}
