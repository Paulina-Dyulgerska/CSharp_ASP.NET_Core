namespace ConformityCheck.Web.ViewModels.ContentDelivery
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleIdSupplierIdInputModel
    {
        [ArticleEntityAttribute]
        public string ArticleId { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }
    }
}
