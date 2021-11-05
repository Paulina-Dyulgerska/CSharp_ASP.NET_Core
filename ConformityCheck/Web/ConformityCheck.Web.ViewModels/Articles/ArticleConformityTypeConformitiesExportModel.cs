namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;

    public class ArticleConformityTypeConformitiesExportModel : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        public string ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        public int ConformityTypeId { get; set; }

        public string ConformityTypeDescription { get; set; }

        public string ConformityId { get; set; }

        public DateTime? ConformityIssueDate { get; set; }

        public DateTime? ConformityValidityDate { get; set; }

        public DateTime? ConformityRequestDate { get; set; }

        public bool? ConformityIsAccepted { get; set; }

        public bool ConformityIsValid => (this.ConformityIsAccepted ?? false) && this.ConformityValidityDate >= DateTime.UtcNow;

        // not needed since variant 3 is used! This is needed only for variant 1:
        public IEnumerable<ConformityBaseExportModel> Conformities { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ArticleConformityTypeConformitiesExportModel>()
                 //// not needed since variant 3 is used! This is needed only for variant 1:
                 // .ForMember(x => x.Conformities, opt => opt.MapFrom(x => x.Article.Conformities))
                .ForMember(x => x.ConformityId, opt => opt.MapFrom(x => x.Article.Conformities.FirstOrDefault(c => c.ConformityTypeId == x.ConformityTypeId).Id))
                .ForMember(x => x.ConformityIssueDate, opt => opt.MapFrom(x => x.Article.Conformities.FirstOrDefault(c => c.ConformityTypeId == x.ConformityTypeId).IssueDate))
                .ForMember(x => x.ConformityValidityDate, opt => opt.MapFrom(x => x.Article.Conformities.FirstOrDefault(c => c.ConformityTypeId == x.ConformityTypeId).ValidityDate))
                .ForMember(x => x.ConformityRequestDate, opt => opt.MapFrom(x => x.Article.Conformities.FirstOrDefault(c => c.ConformityTypeId == x.ConformityTypeId).RequestDate))
                .ForMember(x => x.ConformityIsAccepted, opt => opt.MapFrom(x => x.Article.Conformities.FirstOrDefault(c => c.ConformityTypeId == x.ConformityTypeId).IsAccepted))
                ;
        }
    }
}
