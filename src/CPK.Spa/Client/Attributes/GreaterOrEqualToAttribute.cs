using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CPK.Spa.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterOrEqualToAttribute : ValidationAttribute
    {
        public string FieldName { get; }
        public string DisplayName { get; }

        public GreaterOrEqualToAttribute(string fieldName, string displayName)
        {
            FieldName = fieldName;
            DisplayName = displayName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(FieldName);
            if (otherPropertyInfo == null)
                return Fail(validationContext);
            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (Comparer.Default.Compare(value, otherPropertyValue) >= 0)
                return ValidationResult.Success;
            return Fail(validationContext);
        }

        private ValidationResult Fail(ValidationContext validationContext)
        {
            return new ValidationResult($"Укажите значение которое больше или равно {DisplayName}",
                new[] { validationContext.MemberName });
        }
    }
}