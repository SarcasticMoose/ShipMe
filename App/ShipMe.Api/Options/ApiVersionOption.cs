using Asp.Versioning;

namespace ShipMe.Api.Options;

public class ApiVersionOption
{
    public List<ApiVersion> Supported { get; set; } = [];
    public List<ApiVersion> Depracted { get; set; } = [];
}