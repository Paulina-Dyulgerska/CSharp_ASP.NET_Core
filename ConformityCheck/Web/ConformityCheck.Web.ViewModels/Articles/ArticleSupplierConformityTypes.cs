namespace ConformityCheck.Web.ViewModels.Articles
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleSupplierConformityTypes : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        [ConformityTypeEntityAttribute]
        public int Id { get; set; }

        public string Description { get; set; }

        //[ConformityEntityAttribute]
        public string ConformityId { get; set; }

        public bool ConformityIsAccepted { get; set; }

        public bool ConformityIsValid { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        public bool SupplierConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ArticleSupplierConformityTypes>()
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
                opt => opt.MapFrom(x => x.Conformity.IsAccepted))
                .ForMember(
                x => x.ConformityIsValid,
                opt => opt.MapFrom(x => x.Conformity.IsValid))
                .ForMember(
                x => x.SupplierId,
                opt => opt.MapFrom(x => x.Conformity.SupplierId));
        }
    }
}
