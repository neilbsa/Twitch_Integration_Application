using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Application.Chats.DTOs;
using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;

namespace TwitchLive.Application.Channels.GetChatsToChannel;

public sealed class GetChatToChannelQueryHandler
{
    private readonly IChatService _chatService;

    public GetChatToChannelQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Result<List<ChannelMessageDTO>>> Handle(GetChatToChannelQuery query , CancellationToken token)
    {
        if(query.login.Value == null)
            return Result.Failure<List<ChannelMessageDTO>>(ChannelErrors.ChannelNotFound);

        var chats =await _chatService.GetChatsToChannel(query.login);
        return Result.Success(chats);
    }
}