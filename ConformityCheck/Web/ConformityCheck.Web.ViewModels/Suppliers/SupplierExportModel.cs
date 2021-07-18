namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierExportModel : IMapFrom<ArticleSupplier>, IMapFrom<Supplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPersonFirstName { get; set; }

        public string ContactPersonLastName { get; set; }

        public bool IsMainSupplier { get; set; }

        public bool HasAllConformed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, SupplierExportModel>()
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
                x => x.Email,
                opt => opt.MapFrom(x => x.Supplier.Email))
                .ForMember(
                x => x.PhoneNumber,
                opt => opt.MapFrom(x => x.Supplier.PhoneNumber))
                .ForMember(
                x => x.ContactPersonFirstName,
                opt => opt.MapFrom(x => x.Supplier.ContactPersonFirstName))
                .ForMember(
                x => x.ContactPersonLastName,
                opt => opt.MapFrom(x => x.Supplier.ContactPersonLastName))
                .ForMember(
                x => x.IsMainSupplier,
                opt => opt.MapFrom(x => x.IsMainSupplier));
        }
    }
}
