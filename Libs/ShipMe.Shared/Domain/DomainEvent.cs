namespace ShipMe.Shared.Domain;

public record DomainEvent : IDomainEvent
{
    public Uuid Id { get; } = new Uuid();
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}