namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchEmail(string Value)
{
    public static implicit operator string(UserTwitchEmail userTwitchEmail) => userTwitchEmail.Value;
    public static implicit operator UserTwitchEmail(string value) => new UserTwitchEmail(value);
}
