using ShipMe.Assembly.Abstraction;

namespace ShipMe.Assembly;

internal class CurrentAssemblyProvider(System.Reflection.Assembly assembly) : 
    IVersionAssemblyProvider
{
    public Version? GetVersion()
    {
        return assembly
            .GetName()
            .Version;
    }
}