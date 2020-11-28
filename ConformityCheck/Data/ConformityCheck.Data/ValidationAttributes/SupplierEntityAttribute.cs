namespace ConformityCheck.Data.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using ConformityCheck.Data;

    public class SupplierEntityAttribute : ValidationAttribute
    {

        public SupplierEntityAttribute(string id)
        {
            this.ErrorMessage = $"No such supplier";
            this.Id = id;
        }

        public string Id { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var supplierEntity = context.Suppliers.Any(x => x.Id == this.Id);

            if (supplierEntity)
            {
                return new ValidationResult(this.ErrorMessage);

            }

            return ValidationResult.Success;
        }
    }
}
