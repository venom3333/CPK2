using System.Collections.Generic;
using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public class Line<T>
    {
        public Line(T product, uint quantity)
        {
            Validator.Begin(product, nameof(product))
                .Validate(x => x != null && !x.Equals(default),
                    x => new ValidationError(ValidationErrorCode.IsDefault.ToString("G"), x,
                        new Dictionary<string, object>()));
            Product = product;
            Quantity = quantity;
        }

        public T Product { get; }
        public uint Quantity { get; private set; }

        public void Add() => Quantity++;

        public void Remove()
        {
            Validator
                .Begin(Quantity, nameof(Quantity))
                .IsGreater(0u)
                .ThrowApiException(nameof(Line<T>), nameof(Line<T>));
            Quantity--;
        }
    }
}
