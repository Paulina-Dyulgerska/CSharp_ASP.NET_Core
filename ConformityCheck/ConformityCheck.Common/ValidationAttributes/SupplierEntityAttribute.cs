namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class SupplierEntityAttribute : ValidationAttribute
    {

        public SupplierEntityAttribute()
        {
            this.ErrorMessage = $"No such supplier.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var supplierEntity = context.SupplierEntityIdCheck(value.ToString());

            if (!supplierEntity)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
