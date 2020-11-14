namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class SubstanceRegulationList : BaseDeletableModel<int>
    {
        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }

        public int RegulationListId { get; set; }

        public virtual RegulationList RegulationList { get; set; }

        public string Restriction { get; set; }
    }
}
