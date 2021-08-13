namespace ConformityCheck.Web.ViewModels.Conformities
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityGetInputModel
    {
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        [ArticleEntityAttribute]
        public string ArticleId { get; set; }

        public string CallerViewName { get; set; }
    }
}
