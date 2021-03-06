﻿namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ConformityTypeEntityDescriptionDublicatedAttribute : ValidationAttribute
    {
        public ConformityTypeEntityDescriptionDublicatedAttribute()
        {
            this.ErrorMessage = $"There is already a conformity type with this description.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasSuchDescription = context
                .ConformityTypeEntityDescriptionCheck(value.ToString());

            if (hasSuchDescription)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
