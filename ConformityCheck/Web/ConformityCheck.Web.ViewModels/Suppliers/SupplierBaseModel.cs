namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public abstract class SupplierBaseModel : IMapFrom<Supplier>
    {
        // TODO - to generate numbers automatically
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*$", ErrorMessage = "The field could contain only letters, digits or '-'.")]
        [SupplierNumberDublicatedAttribute]
        [Display(Name = "* Supplier number")]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field could contain only letters, digits, '-', '_' or ' '.")]
        [SupplierNameDublicatedAttribute]
        [Display(Name = "* Supplier name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [RegularExpression(
            "^(?:[a-zA-Z0-9][a-zA-Z0-9_.-]+@(?:[a-zA-Z0-9-_]{2,}[.][a-zA-Z0-9-_]{2,}))(?:.[a-zA-Z0-9-_]{2,})?$",
            ErrorMessage = "The field is not a valid e-mail address.")]
        [Display(Name = "* Supplier email")]
        public string Email { get; set; }
    }
}
