namespace ConformityCheck.Web.ViewModels.Substances.ViewComponents
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SubstancesViewComponentModel : IMapFrom<Substance>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string CASNumberAndDescription { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Substance, SubstancesViewComponentModel>()
                 .ForMember(
                x => x.CASNumberAndDescription,
                opt => opt.MapFrom(x => $"{x.CASNumber} - {x.Description}"));
        }
    }
}
