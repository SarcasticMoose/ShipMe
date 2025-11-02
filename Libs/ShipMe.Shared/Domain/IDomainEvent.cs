namespace ShipMe.Shared.Domain;

public interface IDomainEvent
{
    Uuid Id { get; }
    DateTimeOffset Timestamp { get; }
}