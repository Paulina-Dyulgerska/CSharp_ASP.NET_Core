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
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Name could contain only letters, digits, '-', '_' or ' '.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        [RegularExpression("[0-9+ -]*", ErrorMessage = "The field Phone Number could contain only digits, '-', '+' or ' '.")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Z]+[a-z]*$", ErrorMessage = "The field Contact Person first name could contain only letters.")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[A-Z]+[a-z]*$", ErrorMessage = "The field Contact Person last name could contain only letters.")]
        public string ContactPersonLastName { get; set; }

        public string UserEmail { get; set; }
    }
}
