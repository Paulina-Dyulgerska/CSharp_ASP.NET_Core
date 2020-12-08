namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleManageSuppliersModel : ArticleBaseModel, IHaveCustomMappings
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        public IEnumerable<SupplierModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageSuppliersModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
                                                         .OrderByDescending(x => x.IsMainSupplier)
                                                         .ThenBy(x => x.Supplier.Name)));
        }
    }
}
