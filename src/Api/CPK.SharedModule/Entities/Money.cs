
using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public readonly struct Money
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            Value = value;
            Validator
                .Begin(value, nameof(value))
                .NotDefault()
                .ThrowApiException(nameof(Money), nameof(Money));
        }
    }
}
