namespace TwitchLive.Domain.Channels;

public record LeavedToChannelDomainEvent(Guid id) : IDomainEvent;