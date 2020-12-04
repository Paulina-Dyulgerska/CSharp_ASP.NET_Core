namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleManageConformityTypesModel : ArticleBaseInputModel, IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings
    {
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        public IEnumerable<ArticleConformityTypeModel> ConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformityTypesModel>()
                 .ForMember(x => x.ConformityTypes, opt => opt.MapFrom(a => a.ArticleConformityTypes
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
