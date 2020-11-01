namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class RegulationList : IAuditInfo, IDeletableEntity
    {
        public RegulationList()
        {
            this.SubstanceRegulationLists = new HashSet<SubstanceRegulationList>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string SourceURL { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<SubstanceRegulationList> SubstanceRegulationLists { get; set; }
    }
}
