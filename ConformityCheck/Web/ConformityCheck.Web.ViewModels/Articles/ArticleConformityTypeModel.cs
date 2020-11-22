namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleConformityTypeModel : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        public int ConformityTypeId { get; set; }

        public string ConformityTypeDescription { get; set; }

        public string ConformityId { get; set; }

        public bool ConformityIsAccepted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ArticleConformityTypeModel>()
                .ForMember(
                x => x.ConformityTypeId,
                opt => opt.MapFrom(c => c.ConformityType.Id))
                .ForMember(
                x => x.ConformityTypeDescription,
                opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(
                x => x.ConformityId,
                opt => opt.MapFrom(x => x.Conformity.Id))
                .ForMember(
                x => x.ConformityIsAccepted,
                opt => opt.MapFrom(x => x.Conformity.IsAccepted));
        }
    }
}
