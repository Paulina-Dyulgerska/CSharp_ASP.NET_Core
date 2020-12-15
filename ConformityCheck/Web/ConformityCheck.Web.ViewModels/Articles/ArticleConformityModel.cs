namespace ConformityCheck.Web.ViewModels.Articles
{

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleConformityModel : IMapFrom<ArticleConformityType>
    {
        public string ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        public int ConformityTypeId { get; set; }

        public string ConformityTypeDescription { get; set; }

        public Conformity ArticleConformity { get; set; }
        //public ConformityBaseModel ArticleConformity { get; set; }
    }
}
