using TwitchLive.Domain.Followers.Properties;

namespace TwitchLive.Domain.Followers.DomainEvents;

public record NewFollowerCreatedDomainEvent(Guid id) :IDomainEvent;