namespace ConformityCheck.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class SubstanceRegulationList : IAuditInfo, IDeletableEntity
    {
        [ForeignKey(nameof(Substance))]
        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }

        [ForeignKey(nameof(RegulationList))]
        public int RegulationListId { get; set; }

        public virtual RegulationList RegulationList { get; set; }

        public string Restriction { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
