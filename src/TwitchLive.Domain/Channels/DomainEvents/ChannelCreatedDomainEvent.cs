namespace TwitchLive.Domain.Channels.DomainEvents;

public record ChannelCreatedDomainEvent(Channel Channel) : IDomainEvent;
