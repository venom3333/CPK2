namespace CPK.SharedModule.Entities
{
    public readonly struct Text
    {
        public string Value { get; }

        public Text(string value)
        {
            // Validator.Begin(value, nameof(value))
            //     .NotNull()
            //     .NotWhiteSpace()
            //     .ThrowApiException(nameof(Title), nameof(Title));
            Value = value;
        }
    }
}