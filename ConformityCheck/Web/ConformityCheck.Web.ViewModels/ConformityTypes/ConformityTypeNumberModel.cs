namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeNumberModel : IMapFrom<ConformityType>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ConformityType, ConformityTypeNumberModel>().ForMember(
                x => x.Description,
                opt => opt.MapFrom(x => x.Description));
        }
    }
}
