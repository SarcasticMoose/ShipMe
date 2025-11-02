using System.Text.Json;
using LightResults;
using Microsoft.AspNetCore.Mvc;
using ShipMe.Shared.Errors;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace ShipMe.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result, Func<IResult>? onSuccess = null)
    {
        if (!result.IsFailure()) return onSuccess?.Invoke() ?? Results.Ok();
        var problem = result.ToProblemDetails();
        return Results.Problem(problem);
    }

    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess(out var value))
            return onSuccess?.Invoke(value) ?? Results.Ok(value);

        var problem = result.AsFailure().ToProblemDetails();
        return Results.Problem(problem);
    }

    private static ProblemDetails ToProblemDetails(this Result result)
    {
        var (status, title) = result switch
        {
            _ when result.HasError<ForbiddenError>() => (StatusCodes.Status403Forbidden, "Forbidden"),
            _ when result.HasError<NotFoundError>()  => (StatusCodes.Status404NotFound, "Not Found"),
            _ => (StatusCodes.Status400BadRequest, "Bad Request")
        };

        var details = result.Errors.Select(x => new
        {
            Code = x.Message
        });

        return new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{status}",
            Title = title,
            Status = status,
            Detail = JsonSerializer.Serialize(details)
        };
    }
}