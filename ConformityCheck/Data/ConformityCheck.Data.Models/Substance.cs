namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Substance : IAuditInfo, IDeletableEntity
    {
        public Substance()
        {
            this.ArticleSubstances = new HashSet<ArticleSubstance>();
            this.SubstanceRegulationLists = new HashSet<SubstanceRegulationList>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CASNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<SubstanceRegulationList> SubstanceRegulationLists { get; set; }

        public virtual ICollection<ArticleSubstance> ArticleSubstances { get; set; }
    }
}
