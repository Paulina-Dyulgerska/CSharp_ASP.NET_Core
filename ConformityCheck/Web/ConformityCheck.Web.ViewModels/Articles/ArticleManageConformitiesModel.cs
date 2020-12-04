namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;

    public class ArticleManageConformitiesModel : ArticleBaseInputModel, IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings
    {
        public IEnumerable<ArticleConformityTypeModel> ConformityTypes { get; set; }

        public IEnumerable<SupplierModel> Suppliers { get; set; }

        public ConformityCreateInputModel Conformity { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformitiesModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
                                                         .OrderByDescending(x => x.IsMainSupplier)
                                                         .ThenBy(x => x.Supplier.Name)))
                .ForMember(x => x.ConformityTypes, opt => opt.MapFrom(a => a.ArticleConformityTypes
                                                         .OrderBy(x => x.ConformityTypeId)));
        }
    }
}
