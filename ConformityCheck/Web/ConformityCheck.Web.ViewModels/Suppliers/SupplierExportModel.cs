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

        public string ContactPersonName { get; set; }

        // AutoMapper-a moje da vzeme ot Supplier.ArticleSuppliers.Count() i da mi go dade
        // direktno tuk, bez da pravq custom mapping!!! Ako spazwam conventiona za imenata na
        // propertytata, toj shte se opravi s neshta kato Count, Min, Max i t.n.
        public int ArticleSuppliersCount { get; set; }

        public string UserEmail { get; set; }

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
                x => x.ContactPersonName,
                opt => opt.MapFrom(x => $"{x.Supplier.ContactPersonFirstName} {x.Supplier.ContactPersonLastName}"))
                .ForMember(
                x => x.IsMainSupplier,
                opt => opt.MapFrom(x => x.IsMainSupplier));

            // configuration.CreateMap<Supplier, SupplierExportModel>()
            //        .ForMember(x => x.ArticleSuppliersCount, opt => opt.MapFrom(s => s.ArticleSuppliers.Count));
        }
    }
}
