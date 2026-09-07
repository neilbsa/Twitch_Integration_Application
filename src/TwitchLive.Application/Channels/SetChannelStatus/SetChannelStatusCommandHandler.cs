using System.Runtime.CompilerServices;
using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Repository;

namespace TwitchLive.Application.Channels.SetChannelStatus;

public sealed class SetChannelStatusCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChannelRepository _repository;

    public SetChannelStatusCommandHandler(IUnitOfWork unitOfWork, IChannelRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }



    public async Task<Result> Handle(SetChannelStatusCommand command, CancellationToken token)
    {
        var channel =await _repository.GetChannelByTwitchLogin(command.ChannelLogin);

        if(channel ==null)
            return Result.Failure(ChannelErrors.ChannelNotFound);
        

        channel.SetChannelStatus(command.status);
        await _unitOfWork.SaveChangesAsync(token);

        return Result.Success();
    }
}
