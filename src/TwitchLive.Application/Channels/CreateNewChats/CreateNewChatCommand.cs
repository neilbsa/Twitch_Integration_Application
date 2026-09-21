using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.CreateNewChats;
public record CreateNewChatCommand(UserTwitchLogin messageTo,UserTwitchLogin messageFrom, string Message);
