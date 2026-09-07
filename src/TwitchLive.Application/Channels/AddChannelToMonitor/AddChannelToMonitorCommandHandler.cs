using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.TwitchManager;
using Wolverine;

namespace TwitchLive.Application.Channels.AddChannelToMonitor;

public sealed class AddChannelToMonitorCommandHandler
{
    private readonly ITwitchManager _twitchManager;
    private readonly IChannelRepository _channelRepository;
    private readonly ILogger<AddChannelToMonitorCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public AddChannelToMonitorCommandHandler(
        ITwitchManager twitchManager,
        IChannelRepository channelRepository,
        ILogger<AddChannelToMonitorCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _twitchManager = twitchManager;
        _channelRepository = channelRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddChannelToMonitorCommand command, CancellationToken token)
    {
        if(await _channelRepository.ChannelExistsAsync(command.ChannelLogin))
        {
            return Result.Failure(ChannelErrors.ChannelAlreadyExists);
        } 

        var channelDetail =await _twitchManager.GetChannelDetailsByLoginAsync(command.ChannelLogin);
        if(channelDetail == null)
        {
            return Result.Failure(ChannelErrors.TwitchChannelNotValid);
        }

        _channelRepository.Add(channelDetail);
        await _unitOfWork.SaveChangesAsync(token);
        await _twitchManager.AddChannelToMonitor(channelDetail.Login);

        return Result.Success();

    }
}
