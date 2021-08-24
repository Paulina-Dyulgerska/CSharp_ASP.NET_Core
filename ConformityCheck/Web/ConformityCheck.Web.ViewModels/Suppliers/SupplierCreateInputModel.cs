namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierCreateInputModel : SupplierBaseModel
    {
        [MaxLength(20)]
        [RegularExpression("[0-9+ -]*", ErrorMessage = "The field could contain only digits, '-', '+' or ' '.")]
        [Display(Name = "Supplier phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field could contain only letters. Number of letter should be at least 2.")]
        [NameRegExAttribute]
        [Display(Name = "Contact person first name")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field could contain only letters. Number of letter should be at least 2.")]
        [NameRegExAttribute]
        [Display(Name = "Contact person last name")]
        public string ContactPersonLastName { get; set; }
    }
}
