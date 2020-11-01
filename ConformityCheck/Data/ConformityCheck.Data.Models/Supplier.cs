namespace ConformityCheck.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Supplier : BaseDeletableModel<int>
    {
        public Supplier()
        {
            this.ArticleSuppliers = new HashSet<ArticleSupplier>();
        }

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

        public virtual ICollection<Conformity> Conformities { get; set; }

        public virtual ICollection<ArticleSupplier> ArticleSuppliers { get; set; }
    }
}