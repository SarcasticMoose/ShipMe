using Asp.Versioning;
using Asp.Versioning.Builder;
using ShipMe.Api.Options;

namespace ShipMe.Api.Extensions;

public static class VersioningExtensions
{
    public static ApiVersionOption GetApiVersionFromConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection("Versioning");
        var supportedApiVersions = section.GetSection("Supported");
        var deprecatedApiVersions = section.GetSection("Deprecated");
        
        var apiVersions = new ApiVersionOption();
        
        foreach (var supportedApiVersion in supportedApiVersions.GetChildren())
        {
            apiVersions.Supported.Add(new ApiVersion(Convert.ToInt32(supportedApiVersion.Value)));
        }
        
        foreach (var deprecatedApiVersion in deprecatedApiVersions.GetChildren())
        {
            apiVersions.Depracted.Add(new ApiVersion(Convert.ToInt32(deprecatedApiVersion.Value)));
        }
        return apiVersions;
    }
    
    public static ApiVersionSet GetApiVersionSet(
        this WebApplication app)
    {
        var apiVersionsConfiguration = GetApiVersionFromConfiguration(app.Configuration);
        var versionSetBuilder = app
            .NewApiVersionSet();
        
        foreach (var supportedApiVersion in apiVersionsConfiguration.Supported)
        {
            versionSetBuilder.HasApiVersion(supportedApiVersion);
        }
        
        foreach (var deprecatedApiVersion in apiVersionsConfiguration.Depracted)
        {
            versionSetBuilder.HasDeprecatedApiVersion(deprecatedApiVersion);
        }
        
        return versionSetBuilder
            .ReportApiVersions()
            .Build();
    }
}