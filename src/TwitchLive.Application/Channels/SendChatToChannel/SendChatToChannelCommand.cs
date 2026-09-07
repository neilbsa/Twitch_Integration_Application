using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.SendChatToChannel;



public sealed record SendChatToChannelCommand(UserTwitchLogin channel, string Message);
