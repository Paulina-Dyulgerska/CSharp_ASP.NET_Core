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
            this.ArticleConformityTypes = new HashSet<ArticleConformityType>();
            this.ProductConformityTypes = new HashSet<ProductConformityType>();
        }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Conformity> Conformities { get; set; }

        public virtual ICollection<ArticleConformityType> ArticleConformityTypes { get; set; }

        public virtual ICollection<ProductConformityType> ProductConformityTypes { get; set; }
    }
}
