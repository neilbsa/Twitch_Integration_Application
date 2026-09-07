using System.ComponentModel;
using System.Runtime.CompilerServices;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.LeaveChannel;


public record LeaveChannelCommand(UserTwitchLogin ChannelLogin);
