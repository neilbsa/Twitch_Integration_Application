using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.TwitchManager;

namespace TwitchLive.Application.Channels.SendChatToChannel;

public sealed class SendReplyToChannelChatCommandHandler
{
    private readonly ITwitchManager _twitchManager;
    private readonly IChannelRepository _repository;

    public SendReplyToChannelChatCommandHandler(ITwitchManager twitchManager, IChannelRepository repository)
    {
        _twitchManager = twitchManager;
        _repository = repository;
    }


    public async Task<Result> Handle(SendChatToChannelCommand command,CancellationToken token)
    {
        var channelDetail = await _repository.GetChannelByTwitchLogin(command.channel);

        if(channelDetail == null)
            return Result.Failure(ChannelErrors.ChannelNotMonitored);
        
        var canSendChat = channelDetail.CanSendChat();
        if(canSendChat.IsFailure)
            return Result.Failure(canSendChat.Error);

        await _twitchManager.SendChatToChannel(channelDetail.UserTwitchId, command.Message);
        return Result.Success();
    }
}