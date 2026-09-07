namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchDescription(string Value)
{
    public static implicit operator string(UserTwitchDescription userTwitchDescription) => userTwitchDescription.Value;
    public static implicit operator UserTwitchDescription(string value) => new UserTwitchDescription(value);
}
