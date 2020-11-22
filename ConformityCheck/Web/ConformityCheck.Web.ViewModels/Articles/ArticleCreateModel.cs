namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleCreateModel
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        [Display(Name = "Article Nr. (required) *")]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        [Display(Name = "Article description (required) *")]
        public string Description { get; set; }

        public SupplierInputModel Supplier { get; set; }

        [Display(Name = "Select conformity types (required) *")]
        [Required]
        public IEnumerable<int> ConformityTypes { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }
    }
}
