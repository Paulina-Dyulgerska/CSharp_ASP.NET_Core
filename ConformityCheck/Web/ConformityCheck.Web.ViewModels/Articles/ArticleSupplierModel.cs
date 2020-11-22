namespace ConformityCheck.Web.ViewModels.Articles
{
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleSupplierModel : IMapFrom<Supplier>
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsMainSupplier { get; set; }
    }
}
