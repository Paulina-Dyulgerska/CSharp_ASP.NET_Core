namespace ConformityCheck.Web.ViewModels.Conformities
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityDeleteInputModel
    {
        [ConformityEntityAttribute]
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string SupplierId { get; set; }

        public string CallerViewName { get; set; }
    }
}
