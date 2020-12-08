namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityEditModel : ConformityEditBaseModel, IMapFrom<Conformity>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ConformityTypeDescription { get; set; }

        public string SupplierName { get; set; }

        public string SupplierNumber { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        public bool IsAccepted { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Conformity, ConformityEditModel>()
                .ForMember(x => x.ConformityTypeDescription, opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(x => x.SupplierName, opt => opt.MapFrom(x => x.Supplier.Name))
                .ForMember(x => x.SupplierNumber, opt => opt.MapFrom(x => x.Supplier.Number))
                .ForMember(x => x.ArticleDescription, opt => opt.MapFrom(x => x.Article.Description))
                .ForMember(x => x.ArticleNumber, opt => opt.MapFrom(x => x.Article.Number));
        }
    }
}
