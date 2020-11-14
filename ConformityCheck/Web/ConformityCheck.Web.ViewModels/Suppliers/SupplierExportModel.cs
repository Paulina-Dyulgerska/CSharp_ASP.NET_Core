namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierExportModel : IMapFrom<Supplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string NumberAndName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SupplierExportModel>()
                .ForMember(x => x.NumberAndName, opt => opt.MapFrom(s => s.Number + " - " + s.Name));
        }
    }
}
