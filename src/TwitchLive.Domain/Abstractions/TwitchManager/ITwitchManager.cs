using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Domain.TwitchManager;



public interface ITwitchManager
{
    void JoinChannel(string ch);
    Task<List<Channel>> GetJoinedChannelsAsync();
    Task<Channel?> GetChannelDetailsByLoginAsync(UserTwitchLogin login);
    Task LeaveChannelAsync(UserTwitchLogin login);
    Task SendChatToChannel(UserTwitchId id,string Message);
    Task<bool> IsChannelOnline(UserTwitchLogin ch);
    Task AddChannelToMonitor(UserTwitchLogin ch);

}