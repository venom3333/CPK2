using System;

using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public readonly struct Image
    {
        public Guid Value { get; }

        public Image(Guid id)
        {
            Validator.Begin(id, nameof(id))
                .NotDefault()
                .ThrowApiException(nameof(Image), nameof(Image));
            Value = id;
        }
    }
}
