using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Moderation.GetModeratedChannels;
using TwitchLive.Application.Channels.AddChannelToMonitor;
using TwitchLive.Application.Channels.CreateNewChats;
using TwitchLive.Application.Channels.SetChannelStatus;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Infrastructure.Cache;
using Wolverine;





public interface ITwitchChannelService
{
    Task<IReadOnlyList<ModeratedChannel>> GetModeratedChannelsAsync(
        string userId,
        CancellationToken cancellationToken=default);

    Task RegisterChannelAsync(
        ModeratedChannel channel,
        CancellationToken cancellationToken=default);

    Task ChangeStatusAsync(
        string login,
        ChannelStatus status,
        CancellationToken cancellationToken=default);
        Task StoreChatToChannelAsync(
            UserTwitchLogin MessageTo,
            UserTwitchLogin messageFrom,
            string message,
            CancellationToken token=default);
   
   
}

public sealed class TwitchChannelService : ITwitchChannelService
{

    private readonly IMessageBus _bus;
    private readonly ILogger<TwitchChannelService> _logger;
    private readonly TwitchAPI _api;

    public TwitchChannelService(IMessageBus bus, ILogger<TwitchChannelService> logger, TwitchAPI api)
    {
        _bus = bus;
        _logger = logger;
        _api = api;
      
    }
    public async Task ChangeStatusAsync(string login, ChannelStatus status, CancellationToken cancellationToken)
    {
          var command = new SetChannelStatusCommand(new UserTwitchLogin(login), status);
        var bus = _bus;
        var commandResult =  await bus.InvokeAsync<Result>(command);
        if (commandResult.IsSuccess)
        {
            _logger.LogInformation($"{login} updated Status {status}");
        }else
        {
            _logger.LogError($"error updating onlien status {commandResult.Error}");
        }
    }
    public async Task<IReadOnlyList<ModeratedChannel>> GetModeratedChannelsAsync(string userId, CancellationToken cancellationToken)
    {
         List<ModeratedChannel> channels = new List<ModeratedChannel>();
        string? cursor = String.Empty;
        do
        {
            var users=  await _api.Helix.Moderation.GetModeratedChannelsAsync(userId,first:100,cursor);
            channels.AddRange(users.Data);
            if(users != null)
            {
                cursor = users?.Pagination?.Cursor;
            }
        }while(cursor != null);        
        return channels;
    }
    public async Task RegisterChannelAsync(ModeratedChannel channel, CancellationToken cancellationToken)
    {
        var command = new AddChannelToMonitorCommand(new UserTwitchLogin(channel.BroadcasterLogin),true);
        var result = await _bus.InvokeAsync<Result>(command);
        if (result.IsSuccess)
        {
            _logger.LogInformation($"adding {channel.BroadcasterLogin} success");
        }
        else
        {
               _logger.LogError($"adding {channel.BroadcasterLogin} not success: {result.Error}");
        }
    }

    public async Task StoreChatToChannelAsync(UserTwitchLogin MessageTo, UserTwitchLogin messageFrom, string message, CancellationToken token)
    {
       var newCommand = new CreateNewChatCommand(MessageTo,messageFrom,message);
       var sendingMessageResult=  await _bus.InvokeAsync<Result>(newCommand);

        if (sendingMessageResult.IsFailure)
        {
            _logger.LogError($"error sending message: {sendingMessageResult.Error}");

        }
        else
        {
            _logger.LogInformation("message sent");
        }
    }

 

}