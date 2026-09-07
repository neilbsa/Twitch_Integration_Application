using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Channels.DomainEvents;

namespace TwitchLive.Application.Channels.SetChannelStatus;

public sealed class ChannelStatusUpdateDomainEventHandler
{
    private readonly ILogger<ChannelStatusUpdateDomainEventHandler> _logger;

    public ChannelStatusUpdateDomainEventHandler(ILogger<ChannelStatusUpdateDomainEventHandler> logger)
    {
        _logger = logger;
    }


    public async Task Handle(ChannelStatusUpdateDomainEvent @event,CancellationToken token)
    {
        _logger.LogInformation("ChannelStatusUpdateDomainEventHandler is triggered");
    }
}