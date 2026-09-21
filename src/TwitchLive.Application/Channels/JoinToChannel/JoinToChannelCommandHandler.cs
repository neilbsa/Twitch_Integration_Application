
using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.TwitchManager;

namespace TwitchLive.Application.Channels.JoinToChannel;

public sealed class JoinToChannelCommandHandler
{
  
    private readonly ITwitchManager _twitchManager;
    private readonly IChannelRepository _channelRepository;
    private readonly ILogger<JoinToChannelCommandHandler> _logger;

    private readonly IUnitOfWork _unitOfWork;

    public JoinToChannelCommandHandler(
        ITwitchManager twitchManager,
        IChannelRepository channelRepository,
        ILogger<JoinToChannelCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _twitchManager = twitchManager;
        _channelRepository = channelRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(JoinToChannelCommand command,CancellationToken token)
    {
        try
        {

            var channelDetail = await _channelRepository.GetChannelByTwitchLogin(command.Channellogin);
            if(channelDetail == null)
                return Result.Failure(ChannelErrors.ChannelNotMonitored);
            if(channelDetail?.Status != Domain.Channels.Properties.ChannelStatus.Live)
                return Result.Failure(ChannelErrors.ChannelNotOnline);

            _twitchManager.JoinChannel(command.Channellogin);
            channelDetail.JoinChannel();
            
            await _unitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ChannelErrors.ChannelNotFound);
        }
    }
}
