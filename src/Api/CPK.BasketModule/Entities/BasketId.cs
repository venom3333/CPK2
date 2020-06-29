using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.BasketModule.Entities
{
    public readonly struct BasketId : IEquatable<BasketId>
    {
        public string Value { get; }

        public BasketId(string value)
        {
            Validator.Begin(value, nameof(value))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(BasketId), nameof(BasketId));
            Value = value;
        }

        public bool Equals(BasketId other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is BasketId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
