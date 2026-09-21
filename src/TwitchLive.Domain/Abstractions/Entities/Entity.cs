namespace TwitchLive.Domain.Entities;

public class Entity : IEntity
{
  private readonly List<IDomainEvent> _domainEvents = new();
    public Guid Id { get; init; }
    public Entity(Guid id)
    {
        Id = id;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent); 
    public void ClearDomainEvents() => _domainEvents.Clear();

}

