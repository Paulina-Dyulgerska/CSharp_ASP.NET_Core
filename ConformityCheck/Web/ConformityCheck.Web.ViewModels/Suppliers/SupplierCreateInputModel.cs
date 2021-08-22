namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using ConformityCheck.Common.ValidationAttributes;
    using System.ComponentModel.DataAnnotations;

    public class SupplierCreateInputModel : SupplierBaseModel
    {
        [MaxLength(20)]
        [RegularExpression("[0-9+ -]*", ErrorMessage = "The field Phone Number could contain only digits, '-', '+' or ' '.")]
        [Display(Name = "Supplier phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [NameRegExAttribute]
        //[RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field Contact Person first name could contain only letters.")]
        [Display(Name = "Contact person first name")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [NameRegExAttribute]
        //[RegularExpression("^[a-zA-Z]{2,}$", ErrorMessage = "The field Contact Person last name could contain only letters.")]
        [Display(Name = "Contact person last name")]
        public string ContactPersonLastName { get; set; }
    }
}
