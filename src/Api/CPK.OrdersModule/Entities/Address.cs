using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.OrdersModule.Entities
{
    public sealed class Address
    {
        public string Value { get; }

        public Address(string value)
        {
            Validator
                .Begin(value, nameof(value))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(Address), nameof(Address));
            Value = value;
        }
    }
}
