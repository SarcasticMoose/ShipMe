using System.Reflection;
using Carter;
using ShipMe.Api.Extensions;
using ShipMe.Api.Setup;
using ShipMe.Assembly;
using ShipMe.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var versions = VersioningExtensions.GetApiVersionFromConfiguration(builder.Configuration);

builder.Services.AddAssemblyProvider(Assembly.GetExecutingAssembly());
builder.Services.AddVersioning();
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocuments(versions.ToDocumentStrings());

var app = builder.Build();
app.MapVersioning().MapCarter();
app.MapScalar();
app.Run();