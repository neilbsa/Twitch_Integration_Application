using TwitchLive.Domain.Followers.Properties;
namespace TwitchLive.Domain.Followers.DomainEvents;

public record RecognitionStatusChangedDomainEvent(Guid id ) : IDomainEvent;