namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Product : IAuditInfo, IDeletableEntity
    {
        public Product()
        {
            this.ArticleProducts = new HashSet<ArticleProduct>();
            this.ProductConformities = new HashSet<ProductConformity>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }

        public virtual ICollection<ProductConformity> ProductConformities { get; set; }
    }
}
