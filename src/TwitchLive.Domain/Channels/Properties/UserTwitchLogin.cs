namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchLogin(string Value)
{
    public static implicit operator string(UserTwitchLogin userTwitchLogin) => userTwitchLogin.Value;
    public static implicit operator UserTwitchLogin(string value) => new UserTwitchLogin(value);
}
