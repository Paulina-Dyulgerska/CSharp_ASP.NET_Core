namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class SupplierEntityAttribute : ValidationAttribute
    {
        public SupplierEntityAttribute(bool allowNull = false)
        {
            this.ErrorMessage = $"No such supplier.";
            this.AllowNull = allowNull;
        }

        public bool AllowNull { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && this.AllowNull)
            {
                return ValidationResult.Success;
            }

            if (value == null && !this.AllowNull)
            {
                return new ValidationResult(this.ErrorMessage);
            }

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
