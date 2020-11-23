namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleManageConformityTypesModel : ArticleBaseModel, IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings
    {
        public ConformityTypeModel ConformityType { get; set; }

        public IEnumerable<ArticleConformityTypeModel> ConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformityTypesModel>()
                 .ForMember(
                 X => X.ConformityTypes,
                 opt => opt.MapFrom(a => a.ArticleConformityTypes
                 .OrderBy(x => x.ConformityTypeId)
                 //.Select(x => new ArticleConformityTypeModel
                 //{
                 //    Id = x.ConformityTypeId,
                 //    Description = x.ConformityType.Description,
                 //    ConformityId = x.ConformityId,
                 //    ConformityIsAccepted = x.Conformity.IsAccepted,
                 //})
                 ));
        }
    }
}
