namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierArticlesDetailsInputModel : PagingViewModel
    {
        [Required]
        [SupplierEntityAttribute]
        public string Id { get; set; }

        // the choosen article Id:
        // public ArticleSelectedInputModel ArticleSelected { get; set; }
        [Required]
        [ArticleEntityAttribute]
        public string ArticleId { get; set; }

        public string NumberSortParam { get; set; }

        public string DescriptionSortParam { get; set; }

        public string ConformityTypeSortParam { get; set; }

        public string HasConformitySortParam { get; set; }

        public string AcceptedConformitySortParam { get; set; }

        public string ValidConformitySortParam { get; set; }
    }
}
