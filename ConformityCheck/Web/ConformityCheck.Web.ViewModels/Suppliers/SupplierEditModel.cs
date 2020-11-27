namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierEditModel : IMapFrom<Supplier>
    {
        public string Id { get; set; }

        public string Number { get; set; }

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

        //public IEnumerable<> Articles { get; set; }

        //public IEnumerable<> Conformities { get; set; }
    }
}
