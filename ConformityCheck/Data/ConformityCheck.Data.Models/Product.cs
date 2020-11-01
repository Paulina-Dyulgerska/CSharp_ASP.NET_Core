namespace ConformityCheck.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.ArticleProducts = new HashSet<ArticleProduct>();
            this.ProductConformities = new HashSet<ProductConformity>();
        }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }

        public virtual ICollection<ProductConformity> ProductConformities { get; set; }
    }
}
