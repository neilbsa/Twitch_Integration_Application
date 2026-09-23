using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using TwitchLive.Application.Followers.SendRecognitionToFollower;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Followers.DomainEvents;
using TwitchLive.Domain.Followers.Properties;
using TwitchLive.Domain.Followers.Repository;
using Wolverine;
using Wolverine.Logging;

namespace TwitchLive.Application.Followers.CreateNewFollower;


public sealed class NewFollowerCreatedDomainEventHandler
{
    private readonly IFollowerRepository _followerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _bus;
    private readonly ILogger<NewFollowerCreatedDomainEventHandler> _logger;
    public NewFollowerCreatedDomainEventHandler(IFollowerRepository followerRepository, IMessageBus bus, IUnitOfWork unitOfWork, ILogger<NewFollowerCreatedDomainEventHandler> logger)
    {
        _followerRepository = followerRepository;
        _bus = bus;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(NewFollowerCreatedDomainEvent @event,CancellationToken token)
    {
        var followerDetails = await _followerRepository.GetFollowerByIdAsync(@event.id);
        if(followerDetails != null)
        {
            var command = new SendRecognitionToFollowerCommand(followerDetails.FromUserId,followerDetails.FromLogin,followerDetails.ToUserId);
            var resultSending = await _bus.InvokeAsync<Result>(command,token);
            if (resultSending.IsSuccess)
            {
                followerDetails.SetRecognitionStatus(RecognizeStatus.Sent);
                 await _unitOfWork.SaveChangesAsync(token);
            }
            else
            {
                _logger.LogError($"Error sending message: {resultSending.Error}");
                 _logger.LogInformation($"error sending {command.toUserId} : { command.fromUserId}");
            }
        }
    }
}