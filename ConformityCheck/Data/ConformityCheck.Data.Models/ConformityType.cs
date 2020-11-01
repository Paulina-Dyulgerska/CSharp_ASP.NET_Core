namespace ConformityCheck.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class ConformityType : BaseDeletableModel<int>
    {
        public ConformityType()
        {
            this.Conformities = new HashSet<Conformity>();
        }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public virtual ICollection<Conformity> Conformities { get; set; }
    }
}
