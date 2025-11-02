using Asp.Versioning;
using ShipMe.Api.Options;

namespace ShipMe.Api.Extensions;

public static class ApiVersionExtensions
{
    public static string ToDocumentString(this ApiVersion version)
        => $"v{version.MajorVersion}";

    public static IEnumerable<string> ToDocumentStrings(this ApiVersionOption versions)
    {
        return versions.Depracted.Concat(versions.Supported)
            .OrderBy(x => x.MajorVersion)
            .Select(x => x.ToDocumentString());
    }
}