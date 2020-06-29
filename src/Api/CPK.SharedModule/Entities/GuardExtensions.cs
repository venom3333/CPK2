using System.Collections.Generic;
using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public static class GuardExtensions
    {
        public static ValidationResult<T> ThrowApiException<T>(this ValidationResult<T> result, string objectName, string methodName) =>
            result.TrowIfHasErrors(errors => new ApiException(ApiExceptionCode.ArgumentException,
                new Dictionary<string, object>()
                {
                    [nameof(objectName)] = objectName,
                    [nameof(methodName)] = methodName,
                    [nameof(errors)] = errors
                }));
    }
}
