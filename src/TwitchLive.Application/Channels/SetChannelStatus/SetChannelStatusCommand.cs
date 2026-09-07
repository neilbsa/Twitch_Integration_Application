using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.SetChannelStatus;


public record SetChannelStatusCommand(UserTwitchLogin ChannelLogin,ChannelStatus status);
