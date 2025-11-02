using FluentValidation.Results;
using LightResults;
using Xunit;

namespace ShipMe.Validation.Tests;

public class FluentValidationExtensionsTests
{
    private sealed class DummyError(string message) : Error(message);

    [Fact]
    public void ToResult_ShouldReturnFailure_WhenValidationErrorsContainCustomStateErrors()
    {
        // Arrange
        var error1 = new DummyError("Invalid email");
        var error2 = new DummyError("Password too short");

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Must be valid") { CustomState = error1 },
            new ValidationFailure("Password", "Too short") { CustomState = error2 }
        });

        // Act
        var result = validationResult.ToResult();

        // Assert
        Assert.False(result.IsSuccess());
        Assert.Equal([error1, error2], result.Errors);
    }

    [Fact]
    public void ToResult_ShouldSkipValidationFailuresWithoutCustomState()
    {
        // Arrange
        var expectedError = new DummyError("Missing username");

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Username", "Required") { CustomState = expectedError },
            new ValidationFailure("Password", "Required")
        });

        // Act
        var result = validationResult.ToResult();

        // Assert
        Assert.False(result.IsSuccess());
        Assert.Single(result.Errors);
        Assert.Same(expectedError, result.Errors.First());
    }

    [Fact]
    public void ToResult_ShouldReturnEmptyFailure_WhenNoCustomStatesProvided()
    {
        // Arrange
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Invalid"),
            new ValidationFailure("Password", "Too short")
        });

        // Act
        var result = validationResult.ToResult();

        // Assert
        Assert.False(result.IsSuccess());
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ToResult_ShouldHandleEmptyValidationResult()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var result = validationResult.ToResult();

        // Assert
        Assert.False(result.IsSuccess());
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ToResult_ShouldPreserveErrorOrder()
    {
        // Arrange
        var first = new DummyError("First");
        var second = new DummyError("Second");
        var third = new DummyError("Third");

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Field1", "Error 1") { CustomState = first },
            new ValidationFailure("Field2", "Error 2") { CustomState = second },
            new ValidationFailure("Field3", "Error 3") { CustomState = third }
        });

        // Act
        var result = validationResult.ToResult();

        // Assert
        Assert.Equal([first, second, third], result.Errors);
    }
}