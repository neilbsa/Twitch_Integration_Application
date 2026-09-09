namespace TwitchLive.Domain.Entities;

public class Entity : IEntity
{

    public Entity(Guid id)
    {
        Id = id;
    }

    private readonly List<IDomainEvent> _domainEvents = new();
    public Guid Id { get; private set; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents;
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent); 
    public void ClearDomainEvents() => _domainEvents.Clear();

}

