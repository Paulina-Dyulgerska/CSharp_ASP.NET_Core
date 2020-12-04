namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleManageSuppliersModel : ArticleBaseInputModel, IMapFrom<Article>, IHaveCustomMappings
    {
        public string SupplierId { get; set; }

        public IEnumerable<SupplierModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageSuppliersModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
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
