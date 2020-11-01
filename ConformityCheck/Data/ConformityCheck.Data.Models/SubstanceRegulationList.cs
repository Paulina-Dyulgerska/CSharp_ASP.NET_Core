namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class SubstanceRegulationList : BaseDeletableModel<int>
    {
        [ForeignKey(nameof(Substance))]
        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }

        [ForeignKey(nameof(RegulationList))]
        public int RegulationListId { get; set; }

        public virtual RegulationList RegulationList { get; set; }

        public string Restriction { get; set; }
    }
}
