namespace ConformityCheck.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Substance : BaseDeletableModel<int>
    {
        public Substance()
        {
            this.ArticleSubstances = new HashSet<ArticleSubstance>();
            this.SubstanceRegulationLists = new HashSet<SubstanceRegulationList>();
        }

        [Required]
        [MaxLength(20)]
        public string CASNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<SubstanceRegulationList> SubstanceRegulationLists { get; set; }

        public virtual ICollection<ArticleSubstance> ArticleSubstances { get; set; }
    }
}
