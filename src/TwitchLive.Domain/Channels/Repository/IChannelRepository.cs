using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Domain.Channels.Repository;

public interface IChannelRepository
{
    void Add(Channel ch);
    Task<Channel?> GetChannelById(Guid id);
     Task <Channel?> GetChannelByTwitchLogin(UserTwitchLogin login);
    Task <Channel?> GetChannelByTwitchId(UserTwitchId id);
    Task<IReadOnlyList<Channel>> GetAllMonitoredChannelAsync();
    Task<bool> ChannelExistsAsync(UserTwitchLogin channelLogin);

}