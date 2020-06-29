using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductsModule.Entities
{
    public readonly struct ProductId : IEquatable<ProductId>
    {
        public Guid Value { get; }

        public ProductId(Guid value)
        {
            Value = value;
            Validator
                .Begin(value, nameof(value))
                .NotDefault()
                .ThrowApiException(nameof(ProductId), nameof(ProductId));
        }

        public bool Equals(ProductId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is ProductId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ProductId lhs, ProductId rhs) => lhs.Value == rhs.Value;
        public static bool operator !=(ProductId lhs, ProductId rhs) => !(lhs == rhs);
    }
}
