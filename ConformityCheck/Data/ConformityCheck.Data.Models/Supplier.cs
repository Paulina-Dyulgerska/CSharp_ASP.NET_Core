namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Supplier : IAuditInfo, IDeletableEntity
    {
        public Supplier()
        {
            this.ArticleSuppliers = new HashSet<ArticleSupplier>();
        }

        [Key]
        public int Id { get; set; }

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

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Conformity> Conformities { get; set; }

        public virtual ICollection<ArticleSupplier> ArticleSuppliers { get; set; }
    }
}