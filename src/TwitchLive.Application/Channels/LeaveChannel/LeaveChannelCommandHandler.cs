using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.TwitchManager;

namespace TwitchLive.Application.Channels.LeaveChannel;

public sealed class LeaveChannelCommandHandler
{

    private readonly ITwitchManager _twitchManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChannelRepository _repository;

    public LeaveChannelCommandHandler(IChannelRepository repository, IUnitOfWork unitOfWork, ITwitchManager twitchManager)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _twitchManager = twitchManager;
    }

    public async Task<Result> Handle(LeaveChannelCommand command, CancellationToken token)
    {
            var channelDetails =await _repository.GetChannelByTwitchLogin(command.ChannelLogin);
            if(channelDetails == null)
                return Result.Failure(ChannelErrors.ChannelNotFound);

            if(channelDetails.JoinStatus != JoinStatus.Joined)
                return Result.Failure(ChannelErrors.ChannelNotJoined);
            
             _twitchManager.LeaveChannelAsync(channelDetails.Login);
            channelDetails.LeaveChannel();
            await _unitOfWork.SaveChangesAsync(token);


            return Result.Success();

    }
}
