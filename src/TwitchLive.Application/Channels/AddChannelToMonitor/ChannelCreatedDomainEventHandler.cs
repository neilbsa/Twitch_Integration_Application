using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Channels.DomainEvents;

namespace TwitchLive.Application.Channels.AddChannelToMonitor;

public sealed class ChannelCreatedDomainEventHandler
{
    private readonly ILogger<ChannelCreatedDomainEventHandler> _logger;

    public ChannelCreatedDomainEventHandler(ILogger<ChannelCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ChannelCreatedDomainEvent @event,CancellationToken token)
    {
        _logger.LogInformation("ChannelCreatedDomainEventHandler is triggered");
    }
}