namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierNumberExportModel : IMapFrom<Supplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SupplierNumberExportModel>();
        }
    }
}
