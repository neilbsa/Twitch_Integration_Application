using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Channels.DomainEvents;

namespace TwitchLive.Application.Abstractions.DomainEvents;


public sealed class ChannelCreatedDomainEventHandler
{

    private readonly ILogger<ChannelCreatedDomainEventHandler> _logger;

    public ChannelCreatedDomainEventHandler(ILogger<ChannelCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ChannelCreatedDomainEvent @event,CancellationToken token)
    {

        _logger.LogInformation("New Channel Create Domain Event Triggered");
                 
    }
}