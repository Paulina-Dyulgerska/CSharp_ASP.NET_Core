namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierCreateInputModel
    {
        // TODO - to generate numbers authomatically
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*$", ErrorMessage = "The field Nr. could contain only letters, digits or '-'.")]
        [SupplierNumberDublicatedAttribute]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Name could contain only letters, digits, '-', '_' or ' '.")]
        [SupplierNameDublicatedAttribute]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [RegularExpression(
            "^(?:[a-zA-Z0-9][a-zA-Z0-9_.-]+@(?:[a-zA-Z0-9-_]{2,}[.][a-zA-Z0-9-_]{2,}))(?:.[a-zA-Z0-9-_]{2,})?$",
            ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [MaxLength(20)]
        [RegularExpression("[0-9+ -]*", ErrorMessage = "The field Phone Number could contain only digits, '-', '+' or ' '.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Z]+[a-z]*$", ErrorMessage = "The field Contact Person first name could contain only letters.")]
        [Display(Name = "Contact person first name")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Z]+[a-z]*$", ErrorMessage = "The field Contact Person last name could contain only letters.")]
        [Display(Name = "Contact person last name")]
        public string ContactPersonLastName { get; set; }
    }
}
