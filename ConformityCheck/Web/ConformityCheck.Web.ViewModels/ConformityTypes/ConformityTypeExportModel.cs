namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeExportModel : IMapFrom<Article>, IMapFrom<ConformityType>, IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        // TODO : remove attribute
        [ConformityTypeEntityAttribute]
        public int Id { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool SupplierConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ConformityTypeExportModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.ConformityTypeId))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.ConformityType.Description))
                ;
        }
    }
}
