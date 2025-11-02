using System.Text.Json;
using LightResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ShipMe.Api.Extensions;
using ShipMe.Shared.Errors;
using Xunit;

namespace ShipMe.Api.Tests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToHttpResult_ShouldReturnOk_WhenResultIsSuccess_WithoutOnSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        Assert.IsType<Ok>(httpResult);
    }

    [Fact]
    public void ToHttpResult_ShouldInvokeOnSuccess_WhenProvided()
    {
        // Arrange
        var result = Result.Success();
        var called = false;

        // Act
        var httpResult = result.ToHttpResult(() =>
        {
            called = true;
            return Results.NoContent();
        });

        // Assert
        Assert.True(called);
        Assert.IsType<NoContent>(httpResult);
    }

    [Fact]
    public void ToHttpResult_ShouldReturnForbiddenProblem_WhenHasForbiddenError()
    {
        // Arrange
        var result = Result.Failure([new ForbiddenError()]);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        Assert.Equal(StatusCodes.Status403Forbidden, problem.ProblemDetails.Status);
        Assert.Equal("Forbidden", problem.ProblemDetails.Title);
        Assert.Contains("FORBIDDEN_ACCESS", problem.ProblemDetails.Detail);
    }

    [Fact]
    public void ToHttpResult_ShouldReturnNotFoundProblem_WhenHasNotFoundError()
    {
        // Arrange
        var result = Result.Failure([new NotFoundError()]);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        Assert.Equal(StatusCodes.Status404NotFound, problem.ProblemDetails.Status);
        Assert.Equal("Not Found", problem.ProblemDetails.Title);
        Assert.Contains("NOT_FOUND", problem.ProblemDetails.Detail);
    }

    [Fact]
    public void ToHttpResult_ShouldReturnBadRequestProblem_WhenGenericError()
    {
        // Arrange
        var result = Result.Failure([new InternalError()]);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.ProblemDetails.Status);
        Assert.Equal("Bad Request", problem.ProblemDetails.Title);
        Assert.Contains("INTERNAL_ERROR", problem.ProblemDetails.Detail);
    }

    [Fact]
    public void ToHttpResultT_ShouldReturnOkWithValue_WhenSuccess_WithoutOnSuccess()
    {
        // Arrange
        var result = Result.Success(123);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var ok = Assert.IsType<Ok<int>>(httpResult);
        Assert.Equal(123, ok.Value);
    }

    [Fact]
    public void ToHttpResultT_ShouldReturnProblem_WhenFailure()
    {
        // Arrange
        var result = Result.Failure<int>([new InternalError()]);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.ProblemDetails.Status);
        Assert.Contains("INTERNAL_ERROR", problem.ProblemDetails.Detail);
    }

    [Fact]
    public void ToHttpResultT_ShouldReturnForbidden_WhenFailureHasForbiddenError()
    {
        // Arrange
        var result = Result.Failure<int>([new ForbiddenError()]);

        // Act
        var httpResult = result.ToHttpResult();

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        Assert.Equal(StatusCodes.Status403Forbidden, problem.ProblemDetails.Status);
        Assert.Equal("Forbidden", problem.ProblemDetails.Title);
        Assert.Contains("FORBIDDEN_ACCESS", problem.ProblemDetails.Detail);
    }
}