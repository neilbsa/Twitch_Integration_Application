using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Domain.Channels.DomainEvents   ;

public record ChannelStatusUpdateDomainEvent(Guid id, ChannelStatus status) : IDomainEvent;