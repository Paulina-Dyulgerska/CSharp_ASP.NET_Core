namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ConformityTypeEntityAttribute : ValidationAttribute
    {
        public ConformityTypeEntityAttribute()
        {
            this.ErrorMessage = $"No such conformity type.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var conformityTypeEntity = context.ConformityTypeEntityIdCheck(int.Parse(value.ToString()));

            if (!conformityTypeEntity)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
