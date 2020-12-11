namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleCreateInputModel : ArticleBaseModel
    {
        [SupplierEntityAttribute(allowNull: true)]
        public string SupplierId { get; set; }

        [Required]
        [Display(Name = "* Select conformity type/s")]
        [ConformityTypeEntityAttribute]
        public IEnumerable<int> ConformityTypes { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }
    }
}
