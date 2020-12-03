using ConformityCheck.Common.ValidationAttributes;

namespace ConformityCheck.Web.ViewModels.Articles
{
    public class ArticleManageSuppliersInputModel
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }
    }
}
