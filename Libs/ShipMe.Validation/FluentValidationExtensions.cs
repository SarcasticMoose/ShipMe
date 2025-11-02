using FluentValidation.Results;
using LightResults;

namespace ShipMe.Validation;

public static class FluentValidationExtensions
{
    public static Result ToResult(this ValidationResult result)
    {
        Queue<IError> errors = [];
        foreach (var error in result.Errors)
        {
            var state = error.CustomState as IError;
            if (state == null) continue;
            errors.Enqueue(state);
        }
        return Result.Failure(errors);
    }
}