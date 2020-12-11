namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class SupplierNumberDublicatedAttribute : ValidationAttribute
    {
        public SupplierNumberDublicatedAttribute()
        {
            this.ErrorMessage = $"There is already a supplier with this number.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasName = context
                .SupplierEntityNumberCheck(value.ToString());

            if (hasName)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
