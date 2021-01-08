namespace ConformityCheck.Web.ViewModels.Articles
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    // TODO - delete it and replace it with ArticleExportModel!!!!Check the mapping!!!
    public class ArticleBySupplierModel : IMapFrom<ArticleSupplier>, IHaveCustomMappings
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, ArticleBySupplierModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(a => a.ArticleId))
                .ForMember(
                x => x.Description,
                opt => opt.MapFrom(a => a.Article.Description))
                .ForMember(
                x => x.Number,
                opt => opt.MapFrom(a => a.Article.Number));
        }
    }
}
