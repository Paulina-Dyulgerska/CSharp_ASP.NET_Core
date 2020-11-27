namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class Conformity : BaseDeletableModel<string>
    {
        public Conformity()
        {
            this.ArticleConformityTypes = new HashSet<ArticleConformityType>();
            this.ProductConformities = new HashSet<ProductConformity>();
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(ConformityType))]
        public int ConformityTypeId { get; set; }

        public virtual ConformityType ConformityType { get; set; }

        [Required]
        [ForeignKey(nameof(Supplier))]
        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsValid { get; set; }

        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }

        public virtual ICollection<ArticleConformityType> ArticleConformityTypes { get; set; }

        public virtual ICollection<ProductConformity> ProductConformities { get; set; }
    }
}
