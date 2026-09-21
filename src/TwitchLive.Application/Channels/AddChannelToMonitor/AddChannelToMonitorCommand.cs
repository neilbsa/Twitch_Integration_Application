using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.AddChannelToMonitor;


public record AddChannelToMonitorCommand(UserTwitchLogin ChannelLogin,bool isModerated);
