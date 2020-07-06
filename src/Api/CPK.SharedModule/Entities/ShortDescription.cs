namespace CPK.SharedModule.Entities
{
    public readonly struct ShortDescription
    {
        public string Value { get; }

        public ShortDescription(string value)
        {
            // Validator.Begin(value, nameof(value))
            //     .NotNull()
            //     .NotWhiteSpace()
            //     .ThrowApiException(nameof(Title), nameof(Title));
            Value = value;
        }
    }
}