using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using ShipMe.OpenApi.Transformers;

namespace ShipMe.OpenApi;

public static class OpenApiSetup
{
    private static void AddTransformers(this OpenApiOptions options)
    {
        options.AddDocumentTransformer<VersionTransformer>();
    }
    
    public static void AddOpenApiDocuments(
        this IServiceCollection services,
        IEnumerable<string> openApiNames)
    {
        foreach (var version in openApiNames)
        {
            services.AddOpenApi(version, (options) =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
                options.AddTransformers();
            });
        }
    }
}