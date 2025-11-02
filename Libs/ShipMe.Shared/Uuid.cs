namespace ShipMe.Shared;

public struct Uuid()
{
    public Guid Value { get; } = Guid.CreateVersion7();

    public static implicit operator Guid(Uuid id) => id.Value;
}