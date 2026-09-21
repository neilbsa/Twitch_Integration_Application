using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;

namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.EventSubs;

public class TwitchEventSubService : ITwitchEventSubService
{
    private readonly TwitchAPI _api;
    private  readonly ILogger<TwitchEventSubService> _logger;

    public TwitchEventSubService(TwitchAPI api, ILogger<TwitchEventSubService> logger)
    {
        _api = api;
        _logger = logger;
    }

    private async Task SubscribeAsync(
        string sessionId,
        string type,
        Dictionary<string, string> condition)
    {
         var response = await _api.Helix.EventSub.CreateEventSubSubscriptionAsync(
            type: type,
            version: "1",
            condition: condition,
            method: EventSubTransportMethod.Websocket,
            websocketSessionId: sessionId);


    }
    private async Task SubscribeAsyncV2(
        string sessionId,
        string type,
        Dictionary<string, string> condition)
    {
         var response = await _api.Helix.EventSub.CreateEventSubSubscriptionAsync(
            type: type,
            version: "2",
            condition: condition,
            method: EventSubTransportMethod.Websocket,
            websocketSessionId: sessionId);





    }

    public async Task SubscribeChatAsync(
        string sessionId,
        string broadcasterUserId,
        string botUserId)
    {
        await SubscribeAsync(
            sessionId,
            "channel.chat.message",
            new Dictionary<string, string>
            {
                ["broadcaster_user_id"] = broadcasterUserId,
                ["user_id"] = botUserId
            });
    }

    public async Task SubscribeStreamAsync(
        string sessionId,
        string broadcasterUserId)
    {
        var condition = new Dictionary<string, string>
        {
            ["broadcaster_user_id"] = broadcasterUserId
        };

        await SubscribeAsync(
            sessionId,
            "stream.online",
            condition);

        await SubscribeAsync(
            sessionId,
            "stream.offline",
            condition);
        

    }

    public async Task SubscribeFollowsAsync(
        string sessionId,
        string broadcasterUserId,
        string moderatorId)
    {
        var condition = new Dictionary<string, string>
        {
            ["broadcaster_user_id"] = broadcasterUserId,
            ["moderator_user_id"] =moderatorId
        };

        await SubscribeAsyncV2(
            sessionId,
            "channel.follow",
            condition);

    }
}