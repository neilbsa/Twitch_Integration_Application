using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwitchLib.Api.Helix.Models.Moderation.GetModeratedChannels;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using TwitchLib.EventSub.Core.EventArgs.Stream;
using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Application.Channels.CreateNewChats;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Infrastructure.TwitchManagerSettings;
using TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.EventSubs;
using Wolverine;

public interface ITwitchSubscriptionManager
{
    Task ChannelChatMessages(object? sender, ChannelChatMessageArgs e);
    Task ChannelFollows(object? sender, ChannelFollowArgs e);
    Task ChannelOffline(object? sender, StreamOfflineArgs e);
    Task ChannelOnline(object? sender, StreamOnlineArgs e);
    Task SubscribeAsync(string sessionId, CancellationToken cancellationToken);
}

public sealed class TwitchSubscriptionManager : ITwitchSubscriptionManager

{
    private readonly ITwitchEventSubService _eventSub;
    private readonly IChatService _chatService;
    private readonly IOptions<TwitchOptions> _options;
    private readonly ILogger<TwitchSubscriptionManager> _logger;
    private readonly ITwitchChannelService _channelServices;
    private readonly IMessageBus _bus;
    public TwitchSubscriptionManager(
        ITwitchEventSubService eventSub,
        IOptions<TwitchOptions> options,
        ILogger<TwitchSubscriptionManager> logger,
        ITwitchChannelService channelServices,
        IChatService chatService,
        IMessageBus bus)
    {
        _eventSub = eventSub;
        _options = options;
        _logger = logger;
        _channelServices = channelServices;
        _chatService = chatService;
        _bus = bus;
    }


    public async Task ChannelOffline(object? sender, StreamOfflineArgs e)
    {
        var channelLogin = e.Payload.Event.BroadcasterUserLogin;
        var status = ChannelStatus.Offline;
        await _channelServices.ChangeStatusAsync(channelLogin, status);
    }
    public async Task ChannelOnline(object? sender, StreamOnlineArgs e)
    {
        var channelLogin = e.Payload.Event.BroadcasterUserLogin;
        var status = ChannelStatus.Live;
        await _channelServices.ChangeStatusAsync(channelLogin, status);
    }
    public async Task ChannelFollows(object? sender, ChannelFollowArgs e)
    {
        var broadcasterUserName = e.Payload.Event.BroadcasterUserName;
        var ChatterUsername = e.Payload.Event.UserName;
        _logger.LogInformation($"In Channel {broadcasterUserName} User: {ChatterUsername} followed !");

    }
    public async Task ChannelChatMessages(object? sender, ChannelChatMessageArgs e)
    {
        var broadcasterUserName = new UserTwitchLogin(e.Payload.Event.BroadcasterUserName);
        var ChatterUsername = new UserTwitchLogin(e.Payload.Event.ChatterUserName);
        var IsSubscriber = e.Payload.Event.IsSubscriber;
        var IsModerator = e.Payload.Event.IsModerator;
        var Message = e.Payload.Event.Message.Text;

        _logger.LogInformation($"In Channel {broadcasterUserName} chatter: {ChatterUsername} which is Subscriber: {IsSubscriber} and moderator: {IsModerator} says: {Message}");
    
       var newCommand = new CreateNewChatCommand(broadcasterUserName,ChatterUsername,Message);
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

    public async Task SubscribeAsync(
        string sessionId,
        CancellationToken cancellationToken)
    {
        var botId = _options.Value.BotId;

        var channels =
            await _channelServices.GetModeratedChannelsAsync(
                botId,
                cancellationToken);

        foreach (var channel in channels)
        {
            await SubscribeChannelAsync(
                sessionId,
                botId,
                channel,
                cancellationToken);
        }
    }

    private async Task SubscribeChannelAsync(
        string sessionId,
        string botId,
        ModeratedChannel channel,
        CancellationToken cancellationToken)
    {
        await _eventSub.SubscribeChatAsync(
            sessionId,
            channel.BroadcasterId,
            botId);

        await _eventSub.SubscribeFollowsAsync(
            sessionId,
            channel.BroadcasterId,
            botId);

        await _eventSub.SubscribeStreamAsync(
            sessionId,
            channel.BroadcasterId);

        await _channelServices.RegisterChannelAsync(
            channel,
            cancellationToken);

        _logger.LogInformation(
            "Subscribed to Twitch channel {Channel}",
            channel.BroadcasterLogin);
    }
}