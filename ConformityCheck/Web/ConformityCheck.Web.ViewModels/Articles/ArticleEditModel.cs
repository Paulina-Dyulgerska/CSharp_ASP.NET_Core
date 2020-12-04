namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleEditModel : ArticleBaseModel, IHaveCustomMappings
    {
        public bool IsConfirmed { get; set; }

        public IEnumerable<SupplierModel> Suppliers { get; set; }

        public IEnumerable<ConformityTypeConformitySupplierModel> ConformityTypes { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        public IEnumerable<string> ArticleConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleEditModel>()
                .ForMember(
                x => x.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)))
                .ForMember(
                x => x.IsConfirmed,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .All(x => x.Conformity != null && x.Conformity.IsAccepted)))
                .ForMember(
                x => x.ConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .OrderBy(x => x.ConformityTypeId)))
                .ForMember(
                x => x.ArticleConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                $"{x.ConformityType.Description} => {x.Conformity.IsAccepted}").ToList()))
                .ForMember(
                x => x.ArticleMissingConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                $"{x.ConformityType.Description} => {x.Conformity != null}").ToList()))
                ;
        }
    }
}
