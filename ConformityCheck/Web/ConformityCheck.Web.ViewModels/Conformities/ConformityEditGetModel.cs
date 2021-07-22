namespace ConformityCheck.Web.ViewModels.Conformities
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityEditGetModel
    {
        [ConformityEntityAttribute(allowNull: true)]
        public string Id { get; set; }

        // [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        // [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        // [ArticleEntityAttribute]
        public string ArticleId { get; set; }
    }
}
