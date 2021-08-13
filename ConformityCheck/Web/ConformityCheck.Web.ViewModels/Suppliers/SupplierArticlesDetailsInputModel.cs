namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierArticlesDetailsInputModel : PagingViewModel
    {
        [Required]
        [SupplierEntityAttribute]
        public string Id { get; set; }
    }
}
