using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.TwitchManager;

namespace TwitchLive.Infrastructure.TwitchManagerSettings;


public sealed class TwitchManager : ITwitchManager
{

    private readonly TwitchAPI _twitchApi;
    private readonly TwitchClient _twitchClient;

    public TwitchManager(TwitchAPI twitchApi, TwitchClient twitchClient)
    {
        _twitchApi = twitchApi;
        _twitchClient = twitchClient;
    
    }

    public Task AddChannelToMonitor(UserTwitchLogin ch)
    {
        throw new NotImplementedException();
    }

    public Task<Channel?> GetChannelDetailsByLoginAsync(UserTwitchLogin login)
    {
        throw new NotImplementedException();
    }

    public Task<List<Channel>> GetJoinedChannelsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsChannelOnline(UserTwitchLogin ch)
    {
        throw new NotImplementedException();
    }

    public void JoinChannel(string ch)
    {
        throw new NotImplementedException();
    }

    public Task LeaveChannelAsync(UserTwitchLogin login)
    {
        throw new NotImplementedException();
    }

    public Task SendChatToChannel(UserTwitchId id, string Message)
    {
        throw new NotImplementedException();
    }
}