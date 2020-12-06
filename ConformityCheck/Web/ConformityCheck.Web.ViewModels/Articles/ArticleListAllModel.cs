namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleListAllModel : ArticleModel, IHaveCustomMappings
    {
        public string MainSupplierName { get; set; }

        public string MainSupplierNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleListAllModel>()
               .ForMember(
                x => x.MainSupplierName,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Name))
                .ForMember(
                x => x.MainSupplierNumber,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Number))
                .ForMember(
                x => x.IsConfirmed,
                opt => opt.MapFrom(a => a.Conformities
                .All(x => x.IsAccepted)));
        }
    }
}
