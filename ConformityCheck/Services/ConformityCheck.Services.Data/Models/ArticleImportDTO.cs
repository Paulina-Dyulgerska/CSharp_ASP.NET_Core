namespace ConformityCheck.Services.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ArticleImportDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        public string Description { get; set; }

        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        public string SupplierNumber { get; set; }

        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        public string SupplierName { get; set; }

        [EmailAddress]
        public string SupplierEmail { get; set; }

        [RegularExpression("[0-9-+ ]")]
        public string SupplierPhoneNumber { get; set; }

        [RegularExpression("[A-Za-z]")]
        public string ContactPersonFirstName { get; set; }

        [RegularExpression("[A-Za-z]")]
        public string ContactPersonLastName { get; set; }

        public string UserId { get; set; }
    }
}
