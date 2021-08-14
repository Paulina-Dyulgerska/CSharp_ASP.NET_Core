namespace ConformityCheck.Web.ViewModels.Suppliers.ViewComponents
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SuppliersViewComponentModel : IMapFrom<Supplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string NameAndNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SuppliersViewComponentModel>()
                .ForMember(
                x => x.NameAndNumber,
                opt => opt.MapFrom(x => $"{x.Name} - {x.Number}"));
        }
    }
}
