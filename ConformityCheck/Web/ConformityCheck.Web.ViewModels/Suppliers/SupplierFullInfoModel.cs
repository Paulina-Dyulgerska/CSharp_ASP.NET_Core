namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierFullInfoModel : SupplierBaseModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ContactPersonName => this.ContactPersonFirstName + " " + this.ContactPersonLastName;

        public int ArticlesCount { get; set; }

        //public int ArticlesConfirmed{ get; set; }

        //public int ArticlesUnconfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SupplierFullInfoModel>()
                .ForMember(x => x.ArticlesCount, opt => opt.MapFrom(s => s.ArticleSuppliers.Count));
            //.ForMember(x=>x.ArticlesConfirmed, opt=>opt.MapFrom(s=>s.ArticleSuppliers
            //.Select(x=>x.Article.ArticleConformityTypes.))
        }
    }
}
