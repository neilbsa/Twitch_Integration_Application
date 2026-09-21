namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.EventSubs;




public interface ITwitchEventSubService
{

    Task SubscribeChatAsync(
        string sessionId,
        string broadcasterUserId,
        string botUserId);

        Task SubscribeFollowsAsync(
        string sessionId,
        string broadcasterUserId,
        string moderatorId);

    Task SubscribeStreamAsync(
        string sessionId,
        string broadcasterUserId);
}
