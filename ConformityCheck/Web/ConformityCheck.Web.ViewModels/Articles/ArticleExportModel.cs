namespace ConformityCheck.Web.ViewModels.Articles
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleExportModel : ArticleBaseModel, IMapFrom<ArticleSupplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, ArticleExportModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(asup => asup.ArticleId))
                .ForMember(x => x.Number, opt => opt.MapFrom(asup => asup.Article.Number))
                .ForMember(x => x.Description, opt => opt.MapFrom(asup => asup.Article.Description));
        }
    }
}
