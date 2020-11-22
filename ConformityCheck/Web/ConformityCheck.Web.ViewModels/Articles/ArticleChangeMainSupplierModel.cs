namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleChangeMainSupplierModel : ArticleBaseModel, IMapFrom<Article>, IHaveCustomMappings
    {
        public string MainSupplierId { get; set; }

        public IEnumerable<ArticleSupplierModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleChangeMainSupplierModel>()
                .ForMember(
                x => x.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers.Select(x => new ArticleSupplierModel
                {
                    Id = x.SupplierId,
                    Name = x.Supplier.Name,
                    Number = x.Supplier.Number,
                    IsMainSupplier = x.IsMainSupplier,
                })));
        }
    }
}
