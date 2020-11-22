namespace ConformityCheck.Web.ViewModels.Articles.ViewComponents
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticlesViewComponentModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string NumberAndDescription { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticlesViewComponentModel>()
                .ForMember(
                    x => x.NumberAndDescription,
                    opt => opt.MapFrom(x => $"{x.Number} - {x.Description}"));
        }
    }
}
