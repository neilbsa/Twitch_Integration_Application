namespace TwitchLive.Application.Abstractions.Chats;

using TwitchLive.Application.Chats.DTOs;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Followers.Properties;

public interface IChatService
{
     Task CreateChatToUser(UserTwitchLogin messageTo,UserTwitchLogin messageFrom, string Message);
     Task <List<ChannelMessageDTO>> GetChatsToChannel(UserTwitchLogin messageTo);
     Task SendThanksToFollowerChatAsync(FromUserId fromUserId,FromLogin fromLogin, ToUserId toUserId);
   
}
