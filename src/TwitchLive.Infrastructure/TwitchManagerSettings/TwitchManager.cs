using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.EventSub.Core.Models;
using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.TwitchManager;

namespace TwitchLive.Infrastructure.TwitchManagerSettings;


public sealed class TwitchManager : ITwitchManager
{

    private readonly TwitchAPI _twitchApi;
    private readonly TwitchClient _client;

    public TwitchManager(TwitchAPI twitchApi, TwitchClient client)
    {
        _twitchApi = twitchApi;
        _client = client;
    }

    public Task AddChannelToMonitor(UserTwitchLogin ch)
    {
        throw new NotImplementedException();
    }




    public async Task<Channel?> GetChannelDetailsByLoginAsync(UserTwitchLogin login)
    {
       var response = await _twitchApi.Helix.Users.GetUsersAsync(logins: new List<string>(){ login.Value });
       var user =  response.Users?.FirstOrDefault();
        if(user == null)
        {
            return default;
        }
        var channel = Channel.Create(
                new UserTwitchId(user.Id),
                new UserTwitchLogin(user.Login),
                new UserTwitchDisplayName(user.DisplayName),
                DateTime.UtcNow,
                new UserTwitchType(user.Type),
                new UserTwitchBroadcasterType(user.BroadcasterType),
                new UserTwitchDescription(user.Description),
                new UserTwitchProfileImageUrl(user.ProfileImageUrl),
                new UserTwitchOfflineImageUrl(user.OfflineImageUrl),
                0,new UserTwitchEmail(user.Email),
                ChannelStatus.Unknown,false);


        return channel;

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
       _client.JoinChannel(ch,true);
    }

    public void LeaveChannelAsync(UserTwitchLogin login)
    {
       _client.LeaveChannel(login.Value);

    }

    public Task SendChatToChannel(UserTwitchId id, string Message)
    {
        throw new NotImplementedException();
    }
}