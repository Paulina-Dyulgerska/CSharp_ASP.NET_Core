namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleFullInfoModel : ArticleBaseInputModel, IMapFrom<Article>, IHaveCustomMappings
    {
        public string MainSupplierName { get; set; }

        public string MainSupplierNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        public IEnumerable<string> ArticleConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleFullInfoModel>()
               .ForMember(
                x => x.MainSupplierName,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Name))
                .ForMember(
                x => x.MainSupplierNumber,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Number))
                .ForMember(
                x => x.ArticleConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                $"{x.ConformityType.Description} => {x.Conformity.IsAccepted}").ToList()))
                .ForMember(
                x => x.ArticleMissingConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                $"{x.ConformityType.Description} => {x.Conformity != null}").ToList()))
                .ForMember(
                x => x.IsConfirmed,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .All(x => x.Conformity != null && x.Conformity.IsAccepted)));
        }
    }
}
