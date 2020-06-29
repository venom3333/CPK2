using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CPK.Spa.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterOrEqualAttribute : ValidationAttribute
    {
        public object Value { get; }

        public GreaterOrEqualAttribute(object value)
        {
            Value = value;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;
            if (Comparer.Default.Compare(value, Value) >= 0)
                return ValidationResult.Success;
            return Fail(validationContext);
        }

        private ValidationResult Fail(ValidationContext validationContext)
        {
            return new ValidationResult($"Укажите значение которое больше или равно {Value}",
                new[] { validationContext.MemberName });
        }
    }
}