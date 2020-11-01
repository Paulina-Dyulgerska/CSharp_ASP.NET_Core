using System.ComponentModel.DataAnnotations;

namespace ConformityCheck.Services.Models
{
    public class SupplierImportDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression("[0-9-+ ]")]
        public string PhoneNumber { get; set; }

        [RegularExpression("[A-Za-z]")]
        public string ContactPersonFirstName { get; set; }

        [RegularExpression("[A-Za-z]")]
        public string ContactPersonLastName { get; set; }
    }
}
