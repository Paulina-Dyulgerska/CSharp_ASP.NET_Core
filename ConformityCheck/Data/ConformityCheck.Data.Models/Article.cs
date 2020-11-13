﻿namespace ConformityCheck.Data.Models
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
            this.ArticleConformities = new HashSet<ArticleConformity>();
            this.ArticleSubstances = new HashSet<ArticleSubstance>();
            this.ArticleSuppliers = new HashSet<ArticleSupplier>();
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

        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }

        public virtual ICollection<ArticleConformity> ArticleConformities { get; set; }

        public virtual ICollection<ArticleSubstance> ArticleSubstances { get; set; }

        public virtual ICollection<ArticleSupplier> ArticleSuppliers { get; set; }
    }
}
