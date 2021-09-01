namespace ConformityCheck.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class RegulationList : BaseDeletableModel<int>
    {
        public RegulationList()
        {
            this.SubstanceRegulationLists = new HashSet<SubstanceRegulationList>();
        }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string SourceURL { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<SubstanceRegulationList> SubstanceRegulationLists { get; set; }
    }
}
