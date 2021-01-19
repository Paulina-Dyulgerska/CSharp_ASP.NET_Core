namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Article : BaseDeletableModel<string>
    {
        public Article()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ArticleProducts = new HashSet<ArticleProduct>();
            this.ArticleSubstances = new HashSet<ArticleSubstance>();
            this.ArticleSuppliers = new HashSet<ArticleSupplier>();
            this.ArticleConformityTypes = new HashSet<ArticleConformityType>();
            this.Conformities = new HashSet<Conformity>();
        }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        // [ForeignKey(nameof(Supplier))]
        // public int? MainSupplierID { get; set; }
        // public virtual Supplier MainSupplier { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsConfirmed { get; set; }

        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }

        public virtual ICollection<ArticleConformityType> ArticleConformityTypes { get; set; }

        public virtual ICollection<ArticleSubstance> ArticleSubstances { get; set; }

        public virtual ICollection<ArticleSupplier> ArticleSuppliers { get; set; }

        public virtual ICollection<Conformity> Conformities { get; set; }
    }
}
