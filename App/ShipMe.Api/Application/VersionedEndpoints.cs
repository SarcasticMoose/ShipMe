using Asp.Versioning;
using Asp.Versioning.Builder;
using Carter;

namespace ShipMe.Api.Application;

/// <summary>
/// Provides a base class for Carter modules that support API versioning.
/// </summary>
public abstract class VersionedEndpoints : CarterModule
{
    private readonly string _baseUrl;
    private readonly ApiVersionSet _apiVersionSet;

    /// <summary>
    /// Gets or sets the collection of supported API versions for this module.
    /// </summary>
    /// <remarks>
    /// Derived classes must override this property to define which API versions are supported.
    /// </remarks>
    protected abstract IEnumerable<ApiVersion> ApiVersions { get; set; }

    /// <summary>
    /// Gets or sets the collection of deprecated API versions for this module.
    /// </summary>
    /// <remarks>
    /// By default, this property is initialized as an empty collection.
    /// </remarks>
    protected IEnumerable<ApiVersion> DeprecatedApiVersions { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionedEndpoints"/> class.
    /// </summary>
    /// <param name="baseUrl">
    /// The base URL segment for this API module, typically used to define the
    /// root path of the endpoint group (for example, <c>"users"</c> or <c>"orders"</c>).
    /// </param>
    /// <remarks>
    /// The constructor builds an <see cref="ApiVersionSet"/> that includes all declared
    /// <see cref="ApiVersions"/> and <see cref="DeprecatedApiVersions"/>.
    /// <para>
    /// This ensures that every derived Carter module has a consistent versioning configuration
    /// applied automatically to its endpoint group.
    /// </para>
    /// </remarks>
    protected VersionedEndpoints(string baseUrl)
    {
        var apiVersionSetBuilder = new ApiVersionSetBuilder(null);
        foreach (var apiVersion in ApiVersions)
        {
            apiVersionSetBuilder.HasApiVersion(apiVersion);
        }

        foreach (var deprecatedApiVersion in DeprecatedApiVersions)
        {
            apiVersionSetBuilder.HasApiVersion(deprecatedApiVersion);
        }

        _apiVersionSet = apiVersionSetBuilder.Build();
        _baseUrl = baseUrl;
    }

    /// <summary>
    /// Creates a versioned route group under the path <c>/api/v{version:apiVersion}</c>.
    /// </summary>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> used to map routes.</param>
    /// <returns>
    /// A <see cref="RouteGroupBuilder"/> instance configured with the current API version set.
    /// </returns>
    private RouteGroupBuilder MapVersionedGroup(IEndpointRouteBuilder app)
    {
        return app
            .MapGroup($"/api/v{{version:apiVersion}}/{_baseUrl}")
            .WithApiVersionSet(_apiVersionSet);
    }

    /// <summary>
    /// Adds versioned routes to the endpoint route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder for the application.</param>
    /// <remarks>
    /// This method maps a versioned route group and delegates endpoint definition
    /// to <see cref="DefineEndpoints(RouteGroupBuilder)"/>.
    /// </remarks>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = MapVersionedGroup(app);
        DefineEndpoints(group);
    }

    /// <summary>
    /// Defines versioned endpoints within the provided route group.
    /// </summary>
    /// <param name="group">The route group associated with the API version set.</param>
    /// <remarks>
    /// Derived classes must implement this method to define the actual API routes
    /// (e.g. using <see cref="RouteGroupBuilder.MapGet"/> or <see cref="RouteGroupBuilder.MapPost"/>).
    /// </remarks>
    protected abstract void DefineEndpoints(RouteGroupBuilder group);
}