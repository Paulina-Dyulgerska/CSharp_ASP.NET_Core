namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleManageSuppliersExportModel : ArticleBaseModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string SupplierId { get; set; }

        public IEnumerable<SupplierExportModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageSuppliersExportModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
                                                         .OrderByDescending(x => x.IsMainSupplier)
                                                         .ThenBy(x => x.Supplier.Name)));
        }
    }
}
