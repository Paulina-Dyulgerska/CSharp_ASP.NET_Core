namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleExportModel : ArticleBaseModel
    {
        public string Id { get; set; }
    }
}

//namespace ConformityCheck.Web.ViewModels.Articles
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;

//    using AutoMapper;
//    using ConformityCheck.Data.Models;
//    using ConformityCheck.Services.Mapping;
//    using ConformityCheck.Web.ViewModels.Conformities;
//    using ConformityCheck.Web.ViewModels.ConformityTypes;

//    public class ArticleExportModel : ArticleBaseModel, IHaveCustomMappings
//    {
//        public string Id { get; set; }

//        public int ConformityTypeId { get; set; }

//        public string ConformityTypeDescription { get; set; }

//        public bool IsConfirmed { get; set; }

//        public string ConformityId { get; set; }

//        public DateTime IssueDate { get; set; }

//        public bool IsValid => this.IsAccepted && this.ValidityDate >= DateTime.UtcNow.Date;

//        public bool IsAccepted { get; set; }

//        public DateTime? ValidityDate { get; set; }

//        public DateTime? AcceptanceDate { get; set; }

//        public string Comments { get; set; }

//        public string UserId { get; set; }

//        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

//        public IEnumerable<ConformityExportModel> Conformities { get; set; }

//        public virtual void CreateMappings(IProfileExpression configuration)
//        {
//            configuration.CreateMap<Article, ArticleExportModel>()
//                .ForMember(
//                x => x.ConformityTypes,
//                opt => opt.MapFrom(a => a.ArticleConformityTypes
//                .OrderBy(x => x.Id)))
//                .ForMember(
//                x => x.Conformities,
//                opt => opt.MapFrom(a => a.Conformities
//                .OrderBy(x => x.Id)));
//        }
//    }
//}
