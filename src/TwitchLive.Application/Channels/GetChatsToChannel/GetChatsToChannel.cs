using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.GetChatsToChannel;

public record GetChatToChannelQuery(UserTwitchLogin login);
