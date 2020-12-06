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
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(ConformityType))]
        public int ConformityTypeId { get; set; }

        public virtual ConformityType ConformityType { get; set; }

        [Required]
        [ForeignKey(nameof(Supplier))]
        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        [Required]
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }
    }
}
