namespace ConformityCheck.Web.ViewModels.Articles
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleManageSuppliersInputModel
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }
    }
}
