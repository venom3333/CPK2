using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductCategoriesModule.Entities
{
    public readonly struct ProductCategoryShortDescription
    {
        public string Value { get; }

        public ProductCategoryShortDescription(string value)
        {
            Validator.Begin(value, nameof(value))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(Title), nameof(Title));
            Value = value;
        }
    }
}