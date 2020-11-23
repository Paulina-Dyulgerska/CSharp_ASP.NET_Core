namespace ConformityCheck.Web.ViewModels.Articles
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleConformityTypeModel : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ConformityId { get; set; }

        public bool ConformityIsAccepted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ArticleConformityTypeModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(c => c.ConformityType.Id))
                .ForMember(
                x => x.Description,
                opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(
                x => x.ConformityId,
                opt => opt.MapFrom(x => x.Conformity.Id))
                .ForMember(
                x => x.ConformityIsAccepted,
                opt => opt.MapFrom(x => x.Conformity.IsAccepted));
        }
    }
}
