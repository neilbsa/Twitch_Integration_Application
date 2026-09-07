using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Channels.Repository;

namespace TwitchLive.Application.Channels.LeaveChannel;

public sealed class LeaveChannelCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChannelRepository _repository;

    public LeaveChannelCommandHandler(IChannelRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LeaveChannelCommand command, CancellationToken token)
    {
            var channelDetails =await _repository.GetChannelByTwitchLogin(command.ChannelLogin);
            if(channelDetails == null)
                return Result.Failure(ChannelErrors.ChannelNotFound);

            if(channelDetails.JoinStatus != JoinStatus.Joined)
                return Result.Failure(ChannelErrors.ChannelNotJoined);
            
            channelDetails.LeaveChannel();
            await _unitOfWork.SaveChangesAsync(token);


            return Result.Success();

    }
}
