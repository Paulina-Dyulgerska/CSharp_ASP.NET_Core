namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleManageConformityTypesInputModel
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }
    }
}
