namespace ConformityCheck.Services.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SupplierImportDTO
    {
        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        public string ContactPersonLastName { get; set; }
    }
}
