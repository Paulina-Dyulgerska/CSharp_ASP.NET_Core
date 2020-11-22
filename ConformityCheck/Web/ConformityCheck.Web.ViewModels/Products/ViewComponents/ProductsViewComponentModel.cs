namespace ConformityCheck.Web.ViewModels.Products.ViewComponents
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ProductsViewComponentModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string NumberAndDescription { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductsViewComponentModel>()
                .ForMember(
                x => x.NumberAndDescription,
                opt => opt.MapFrom(x => $"{x.Number} - {x.Description}"));
        }
    }
}
