namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleManageConformityTypesModel : ArticleModel, IHaveCustomMappings
    {
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        public IEnumerable<ConformityTypeModel> ConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformityTypesModel>()
                 .ForMember(x => x.ConformityTypes, opt => opt.MapFrom(a => a.ArticleConformityTypes
                                                     .OrderBy(x => x.ConformityTypeId)));
        }
    }
}
