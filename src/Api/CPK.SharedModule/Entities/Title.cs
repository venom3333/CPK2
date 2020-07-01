
using FluentValidationGuard;

namespace CPK.SharedModule.Entities
{
    public readonly struct Title
    {
        public string Value { get; }

        public Title(string value)
        {
            Validator.Begin(value, nameof(value))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(Title), nameof(Title));
            Value = value;
        }
    }
}
