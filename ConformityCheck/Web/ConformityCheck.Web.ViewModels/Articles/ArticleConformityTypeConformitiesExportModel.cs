namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleConformityTypeConformitiesExportModel : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        public ArticleBaseExportModel Article { get; set; }

        public ConformityTypeBaseExportModel ConformityType { get; set; }

        public IEnumerable<ConformityBaseExportModel> Conformities { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ArticleConformityTypeConformitiesExportModel>()
                 .ForMember(x => x.Conformities, opt => opt.MapFrom(x => x.Article.Conformities));
        }
    }
}
