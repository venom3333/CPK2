using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductsModule.Entities
{
    public readonly struct Image
    {
        public Guid Id { get; }

        public Image(Guid id)
        {
            Validator.Begin(id, nameof(id))
                .NotDefault()
                .ThrowApiException(nameof(Image), nameof(Image));
            Id = id;
        }
    }
}
