using System;

using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public readonly struct Id : IEquatable<Id>
    {
        public Guid Value { get; }

        public Id(Guid value)
        {
            Value = value;
            Validator
                .Begin(value, nameof(value))
                .NotDefault()
                .ThrowApiException(nameof(Id), nameof(Id));
        }

        public bool Equals(Id other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Id other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Id lhs, Id rhs) => lhs.Value == rhs.Value;
        public static bool operator !=(Id lhs, Id rhs) => !(lhs == rhs);
    }
}
