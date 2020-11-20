namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class EditExportModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public string MainSupplierName { get; set; }

        public string MainSupplierNumber { get; set; }

        public string MainSupplierId { get; set; }

        public SupplierInputModel Supplier { get; set; }

        [Required]
        public IEnumerable<int> ConformityTypes { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }


        // confirmed - not confirmed according to the user or like it is now?
        public bool IsConfirmed { get; set; }

        public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        public IEnumerable<string> ArticleConformityTypes { get; set; }

        public IEnumerable<ArticleSupplierExportModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, EditExportModel>()
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
                X => X.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers.Select(x => new ArticleSupplierExportModel
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
                .All(x => x.Conformity != null && x.Conformity.IsAccepted)));
        }
    }
}
