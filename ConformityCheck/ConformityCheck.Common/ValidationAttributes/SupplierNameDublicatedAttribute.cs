namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class SupplierNameDublicatedAttribute : ValidationAttribute
    {
        public SupplierNameDublicatedAttribute()
        {
            this.ErrorMessage = $"There is already a supplier with this name.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasName = context
                .SupplierEntityNameCheck(value.ToString());

            if (hasName)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
