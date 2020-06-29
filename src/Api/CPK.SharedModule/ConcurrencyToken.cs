using System.Collections.Generic;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.SharedModule
{
    public readonly struct ConcurrencyToken<T>
    {
        public string Token { get; }
        public T Entity { get; }

        public ConcurrencyToken(string token, T entity)
        {
            Validator.Begin(token, nameof(token))
                .NotNull()
                .NotWhiteSpace()
                .Map(entity, nameof(entity))
                .Validate(
                    x => x != null && !x.Equals(default),
                    x => new ValidationError(ValidationErrorCode.IsDefault.ToString("G"), x, new Dictionary<string, object>())
                    )
                .ThrowApiException(nameof(ConcurrencyToken<T>), nameof(ConcurrencyToken<T>));
            Token = token;
            Entity = entity;
        }
    }
}
