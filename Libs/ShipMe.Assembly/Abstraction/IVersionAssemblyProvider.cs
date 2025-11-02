namespace ShipMe.Assembly.Abstraction;

public interface IVersionAssemblyProvider
{
    public Version? GetVersion();
}