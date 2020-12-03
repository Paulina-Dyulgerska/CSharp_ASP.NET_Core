namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class DescriptionDublicatedAttribute : ValidationAttribute
    {
        public DescriptionDublicatedAttribute()
        {
            this.ErrorMessage = $"There is already a conformity type with this description.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasDescription = context
                .ConformityTypeEntityDescriptionCheck(value.ToString());

            if (hasDescription)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
