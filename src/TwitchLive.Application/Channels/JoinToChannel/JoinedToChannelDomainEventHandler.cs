
using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Channels;

namespace TwitchLive.Application.Channels.JoinToChannel;

public sealed class JoinedToChannelDomainEventHandler
{
    private readonly ILogger<JoinedToChannelDomainEventHandler> _logger;

    public JoinedToChannelDomainEventHandler(ILogger<JoinedToChannelDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle (JoinedToChannelDomainEvent @event,CancellationToken token)
    {
        _logger.LogInformation("JoinedToChannelDomainEventHandler triggered");
    }
}