namespace ConformityCheck.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Common.Models;

    public class Supplier : BaseDeletableModel<string>
    {
        public Supplier()
        {
            this.ArticleSuppliers = new HashSet<ArticleSupplier>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string ContactPersonFirstName { get; set; }

        [MaxLength(20)]
        public string ContactPersonLastName { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Conformity> Conformities { get; set; }

        public virtual ICollection<ArticleSupplier> ArticleSuppliers { get; set; }
    }
}
