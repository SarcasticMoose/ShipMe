using System.Collections.Immutable;

namespace ShipMe.Shared.Domain;

public abstract class AggregateRoot : IEntity
{
    private List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToImmutableArray();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public Uuid Id { get; set; }
}