namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierModel : IMapFrom<ArticleSupplier>, IMapFrom<Supplier>, IHaveCustomMappings
    {
        [SupplierEntityAttribute]
        public string Id { get; set; } //Go out!!!

        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsMainSupplier { get; set; }

        public bool HasAllConformed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, SupplierModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(x => x.SupplierId))
                .ForMember(
                x => x.Number,
                opt => opt.MapFrom(x => x.Supplier.Number))
                .ForMember(
                x => x.Name,
                opt => opt.MapFrom(x => x.Supplier.Name))
                .ForMember(
                x => x.IsMainSupplier,
                opt => opt.MapFrom(x => x.IsMainSupplier));
        }
    }
}
