using Microsoft.Extensions.DependencyInjection;
using ShipMe.Assembly.Abstraction;

namespace ShipMe.Assembly;

public static class Setup
{
    public static IServiceCollection AddAssemblyProvider(
        this IServiceCollection services,
        System.Reflection.Assembly assembly)
        => services.AddAssemblyProviderImplementation(assembly);

    private static IServiceCollection AddAssemblyProviderImplementation(
        this IServiceCollection services,
        System.Reflection.Assembly assembly)
    {
        return services.AddSingleton<IVersionAssemblyProvider, CurrentAssemblyProvider>(c => new CurrentAssemblyProvider(assembly));
    }
}