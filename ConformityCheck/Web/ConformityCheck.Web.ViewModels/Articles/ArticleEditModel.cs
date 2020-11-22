namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleEditModel : ArticleBaseModel, IMapFrom<Article>, IHaveCustomMappings
    {
        public string MainSupplierName { get; set; }

        public string MainSupplierNumber { get; set; }

        public string MainSupplierId { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        // confirmed - not confirmed according to the user or like it is now?
        public bool IsConfirmed { get; set; }

        public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        public IEnumerable<string> ArticleConformityTypes { get; set; }

        public IEnumerable<ArticleSupplierModel> Suppliers { get; set; }

        public IEnumerable<ArticleConformityTypeModel> ConformityTypesMainSupplier { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleEditModel>()
                .ForMember(
                x => x.MainSupplierId,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).SupplierId))
               .ForMember(
                x => x.MainSupplierName,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Name))
                .ForMember(
                x => x.MainSupplierNumber,
                opt => opt.MapFrom(a => a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Number))
                .ForMember(
                x => x.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers.Select(x => new ArticleSupplierModel
                {
                    Id = x.SupplierId,
                    Name = x.Supplier.Name,
                    Number = x.Supplier.Number,
                    IsMainSupplier = x.IsMainSupplier,
                })))
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
                .All(x => x.Conformity != null && x.Conformity.IsAccepted)))
                .ForMember(
                x => x.ConformityTypesMainSupplier,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .Where(s => s.Conformity.SupplierId == this.MainSupplierId)
                .Select(x => new ArticleConformityTypeModel
                {
                    ConformityTypeId = x.ConformityType.Id,
                    ConformityTypeDescription = x.ConformityType.Description,
                    ConformityId = x.ConformityId,
                    ConformityIsAccepted = x.Conformity.IsAccepted,
                })));
        }
    }
}
