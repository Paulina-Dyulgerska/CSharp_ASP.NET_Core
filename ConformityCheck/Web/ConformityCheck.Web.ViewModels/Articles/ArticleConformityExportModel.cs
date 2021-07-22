namespace ConformityCheck.Web.ViewModels.Articles
{

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;

    public class ArticleConformityExportModel : IMapFrom<ArticleConformityType>, 
        IMapFrom<Conformity>, 
        IMapFrom<ArticleSupplier>
    {
        public string ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        public int ConformityTypeId { get; set; }

        public string ConformityTypeDescription { get; set; }

        public ConformityValidityExportModel ArticleConformity { get; set; }
    }
}
