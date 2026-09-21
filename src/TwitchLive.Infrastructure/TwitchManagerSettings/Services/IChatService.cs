using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Channels.SendChatMessage;
using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Application.Chats.DTOs;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Followers.Properties;
using TwitchLive.Infrastructure.Cache;

public sealed class ChatService : IChatService
{
    private readonly SemaphoreSlim _rateLimitLock = new SemaphoreSlim(1, 1);
     private readonly ICaching _caching;
    private readonly TwitchAPI _api;
    private readonly ILogger<ChatService> _logger;
    private readonly ConcurrentDictionary<string, object> _channelLocks = new();

    public ChatService(ICaching caching, TwitchAPI api, ILogger<ChatService> logger)
    {
        _caching = caching;
        _api = api;
        _logger = logger;
    }

    private string GenerateChannelKey(UserTwitchLogin login) => $"chat:{login.Value}";

    public async Task CreateChatToUser(UserTwitchLogin messageTo, UserTwitchLogin messageFrom, string Message)
                    {
        var newMessage = new ChannelMessageDTO(
            messageFrom.Value,
            messageTo.Value,
            Message);
        string channelChatKey = GenerateChannelKey(messageTo);

        var channelLock = _channelLocks.GetOrAdd(
            channelChatKey,
            _ => new object());

        List<ChannelMessageDTO> messages = new List<ChannelMessageDTO>();
        lock (channelLock)
    {
            _caching.Get<List<ChannelMessageDTO>>(channelChatKey, out messages);

            var updatedMessages = messages == null
                ? new List<ChannelMessageDTO>()
                : new List<ChannelMessageDTO>(messages);

            updatedMessages.Add(newMessage);

            _logger.LogInformation($"Saving {updatedMessages.Count} messages for channel {channelChatKey}");

            _caching.Set(
                channelChatKey,
                updatedMessages,
                TimeSpan.FromDays(1));
        }
    }

    public async Task<List<ChannelMessageDTO>> GetChatsToChannel(UserTwitchLogin messageTo)
    {
        string channelChatKey = GenerateChannelKey(messageTo);
        List<ChannelMessageDTO> messages = new List<ChannelMessageDTO>();
        _caching.Get<List<ChannelMessageDTO>>(channelChatKey, out messages);

        _logger.LogInformation($"Retrieving {messages?.Count ?? 0} messages for channel {channelChatKey}");

        return messages ?? new List<ChannelMessageDTO>();
    }

    public async Task SendThanksToFollowerChatAsync(FromUserId fromUserId, FromLogin fromLogin, ToUserId toUserId)
    {
        try
        {
            await _rateLimitLock.WaitAsync();

            var newMessage = new SendChatMessageRequest()
            {
                BroadcasterId = toUserId.Value,
                SenderId = fromUserId.Value,
                Message = $"Thanks for the follow {fromLogin.Value}"
            };

            await _api.Helix.Chat.SendChatMessage(newMessage);
            _logger.LogInformation($"success sending {toUserId} : {fromUserId}: {fromLogin.Value}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending in TwitchAPI");
            throw new Exception("Error sending in TwitchAPI", ex);
        }
        finally
        {
            await Task.Delay(5000);
            _rateLimitLock.Release();
        }
    }
}

