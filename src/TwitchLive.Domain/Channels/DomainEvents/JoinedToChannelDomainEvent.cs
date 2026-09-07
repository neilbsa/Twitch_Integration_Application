namespace TwitchLive.Domain.Channels;

public record JoinedToChannelDomainEvent(Guid id) : IDomainEvent;
