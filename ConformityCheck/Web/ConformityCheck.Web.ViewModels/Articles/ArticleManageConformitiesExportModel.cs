namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleManageConformitiesExportModel : ArticleBaseModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

        public IEnumerable<SupplierExportModel> Suppliers { get; set; }

        public ConformityCreateInputModel Conformity { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformitiesExportModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
                                                         .OrderByDescending(x => x.IsMainSupplier)
                                                         .ThenBy(x => x.Supplier.Name)))
                .ForMember(x => x.ConformityTypes, opt => opt.MapFrom(a => a.ArticleConformityTypes
                                                         .OrderBy(x => x.ConformityTypeId)));
        }
    }
}
