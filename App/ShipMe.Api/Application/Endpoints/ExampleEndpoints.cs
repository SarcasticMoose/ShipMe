using Asp.Versioning;
using LightResults;
using ShipMe.Api.Extensions;
using ShipMe.Shared.Errors;

namespace ShipMe.Api.Application.Endpoints;

public class ExampleEndpoints() : VersionedEndpoints("example")
{
    protected override IEnumerable<ApiVersion> ApiVersions { get; set; } = [new(1, 0)];

    protected override void DefineEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("", (CancellationToken ct) => Results.Ok("Everything good!"));
        group.MapGet("/error", (CancellationToken ct) => Result.Failure<InternalError>().ToHttpResult());
    }
}