namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System;
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeModel : IMapFrom<ConformityType>, IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        //[ConformityTypeEntityAttribute]
        public int Id { get; set; } //go out!!!

        public string Description { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // dali da e tuk ili po-dobre supplier-a da si go nosi towa info - da go premestq v suppliermodel!!!
        public bool SupplierConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ConformityTypeModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.ConformityTypeId))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.ConformityType.Description));
        }
    }
}
