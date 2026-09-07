namespace TwitchLive.Domain.Channels.Properties;

public record UserTwitchProfileImageUrl(string Value)
{
    public static implicit operator string(UserTwitchProfileImageUrl userTwitchProfileImageUrl) => userTwitchProfileImageUrl.Value;
    public static implicit operator UserTwitchProfileImageUrl(string value) => new UserTwitchProfileImageUrl(value);
}
