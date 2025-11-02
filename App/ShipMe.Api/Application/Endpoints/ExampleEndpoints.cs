using Carter;

namespace ShipMe.Api.Application.Endpoints;

public class ExampleEndpoints() : CarterModule("/example")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", (HttpContext ctx) => Results.Ok($"Everything good! Version 1.0"))
            .MapToApiVersion(1.0);
        app.MapGet("", (HttpContext ctx) => Results.Ok($"Everything good! Version 2.0"))
            .MapToApiVersion(2.0);
    }
}