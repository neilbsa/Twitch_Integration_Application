using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.SendReplyToChannelChat;



public sealed record SendReplyToChannelChatCommand(UserTwitchLogin channel,string replyTo, string Message);
