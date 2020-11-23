namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleManageSuppliersModel : ArticleBaseModel, IMapFrom<Article>, IHaveCustomMappings
    {
        public SupplierInputModel Supplier { get; set; }

        public IEnumerable<ArticleSupplierModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageSuppliersModel>()
                .ForMember(
                X => X.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)
                //.Select(x => new ArticleSupplierModel
                //{
                //    Id = x.SupplierId,
                //    Name = x.Supplier.Name,
                //    Number = x.Supplier.Number,
                //    IsMainSupplier = x.IsMainSupplier,
                //})
                ));
        }
    }
}
