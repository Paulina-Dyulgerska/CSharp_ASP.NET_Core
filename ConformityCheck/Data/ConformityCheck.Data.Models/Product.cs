namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.ArticleProducts = new HashSet<ArticleProduct>();
            this.ProductConformityTypes = new HashSet<ProductConformityType>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }

        public virtual ICollection<ProductConformityType> ProductConformityTypes { get; set; }
    }
}
