namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleExportViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public string Number { get; set; }

        public string Description { get; set; }

        // confirmed - not confirmed
        public bool IsConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleExportViewModel>().ForMember(
                m => m.IsConfirmed,
                opt => opt.MapFrom(x => x.ArticleConformities.All(ac => ac.Conformity.IsAccepted)));
        }
    }
}
