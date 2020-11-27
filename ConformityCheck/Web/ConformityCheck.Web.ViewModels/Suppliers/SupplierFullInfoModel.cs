namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierFullInfoModel : IMapFrom<Supplier>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPersonName { get; set; }

        public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public int ArticlesCount { get; set; }

        //public int ArticlesConfirmed{ get; set; }

        //public int ArticlesUnconfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SupplierFullInfoModel>()
                .ForMember(x => x.ArticlesCount, opt => opt.MapFrom(s => s.ArticleSuppliers.Count))
                .ForMember(
                    x => x.ContactPersonName,
                    opt => opt.MapFrom(s => s.ContactPersonFirstName + " " + s.ContactPersonLastName));
            //.ForMember(x=>x.ArticlesConfirmed, opt=>opt.MapFrom(s=>s.ArticleSuppliers
            //.Select(x=>x.Article.ArticleConformityTypes.))
        }
    }
}
