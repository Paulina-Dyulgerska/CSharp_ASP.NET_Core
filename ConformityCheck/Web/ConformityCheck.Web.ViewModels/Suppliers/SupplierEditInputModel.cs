﻿namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    public class SupplierEditInputModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        [RegularExpression("[0-9-+ ]*")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        [RegularExpression("[A-Za-z]{2,}")]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        [RegularExpression("[A-Za-z]{2,}")]
        public string ContactPersonLastName { get; set; }

        // TODO: who is the user?
        public string UserId { get; set; }
    }
}
