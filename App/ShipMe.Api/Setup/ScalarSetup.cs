using Asp.Versioning.ApiExplorer;
using Scalar.AspNetCore;

namespace ShipMe.Api.Setup;

public static class ScalarSetup
{
    public static void MapScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var docKey = description.GroupName;
                var displayName = description.IsDeprecated ? $"{docKey}-deprecated" : docKey;
                options
                    .AddDocument(docKey, displayName, $"/openapi/{docKey}.json")
                    .WithTitle(displayName);
            }
        });
    }
}