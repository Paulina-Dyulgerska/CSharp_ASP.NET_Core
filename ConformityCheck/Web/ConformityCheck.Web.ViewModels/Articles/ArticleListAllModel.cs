namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleListAllModel : ArticleBaseModel, IHaveCustomMappings
    {
        public string Id { get; set; }

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
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                                            .All(ac => a.Conformities
                                                        .Any(c => ac.ConformityTypeId == c.ConformityTypeId
                                                                    && c.IsAccepted
                                                                    && c.ValidityDate >= DateTime.UtcNow.Date))));
        }
    }
}
