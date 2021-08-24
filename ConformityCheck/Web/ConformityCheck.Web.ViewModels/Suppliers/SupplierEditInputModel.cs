namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class SupplierEditInputModel : IMapFrom<Supplier>
    {
        [SupplierEntityAttribute]
        public string Id { get; set; }

        [BindNever]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field could contain only letters, digits, '-', '_' or ' '.")]
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

        [MaxLength(20)]
        [RegularExpression("[0-9+ -]*", ErrorMessage = "The field could contain only digits, '-', '+' or ' '.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [NameRegExAttribute]
        [RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field could contain only letters. Number of letter should be at least 2.")]
        [Display(Name = "Contact person first name")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [NameRegExAttribute]
        [RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field could contain only letters. Number of letter should be at least 2.")]
        [Display(Name = "Contact person last name")]
        public string ContactPersonLastName { get; set; }

        [Display(Name = "Creator username")]
        [BindNever]
        public string UserEmail { get; set; }
    }
}
