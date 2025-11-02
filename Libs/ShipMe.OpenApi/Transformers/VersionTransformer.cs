using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using ShipMe.Assembly.Abstraction;

namespace ShipMe.OpenApi.Transformers;

public class VersionTransformer(IVersionAssemblyProvider provider) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var assemblyVersion = provider.GetVersion()?.ToString(3);
        document.Info.Version = assemblyVersion;
        return Task.CompletedTask;
    }
}