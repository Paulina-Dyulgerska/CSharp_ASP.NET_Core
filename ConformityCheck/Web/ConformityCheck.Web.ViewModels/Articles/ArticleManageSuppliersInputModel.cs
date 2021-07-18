namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleManageSuppliersInputModel
    {
        [Required]
        [ArticleEntityAttribute]
        [Display(Name = "Article")]
        public string Id { get; set; }

        // Required is here to activate the Required in the client-side validation.
        [Required]
        [SupplierEntityAttribute]
        [Display(Name = "Supplier")]
        public string SupplierId { get; set; }
    }
}
