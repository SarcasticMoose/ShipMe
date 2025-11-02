using Asp.Versioning;
using ShipMe.Api.Extensions;

namespace ShipMe.Api.Setup;

public static class VersioningSetup
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static RouteGroupBuilder MapVersioning(
        this WebApplication app)
    {
        var apiVersionSet = app.GetApiVersionSet();

        var group = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet)
            .ReportApiVersions();
        return group;
    }
}