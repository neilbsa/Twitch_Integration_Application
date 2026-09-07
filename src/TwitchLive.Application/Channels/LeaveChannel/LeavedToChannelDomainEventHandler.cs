using Microsoft.Extensions.Logging;
using TwitchLive.Domain.Channels;

namespace TwitchLive.Application.Channels.LeaveChannel;

public sealed class LeavedToChannelDomainEventHandler
{
    private readonly ILogger<LeavedToChannelDomainEventHandler> _logger;

    public LeavedToChannelDomainEventHandler(ILogger<LeavedToChannelDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle (LeavedToChannelDomainEvent @event,CancellationToken token)
    {
        _logger.LogInformation("LeavedToChannelDomainEventHandler triggered");
    }
}