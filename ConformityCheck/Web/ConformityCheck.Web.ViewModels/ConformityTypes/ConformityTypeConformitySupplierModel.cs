﻿namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeConformitySupplierModel : IMapFrom<ArticleConformityType>, IHaveCustomMappings
    {
        [ConformityTypeEntityAttribute]
        public int Id { get; set; }

        public string Description { get; set; }

        //[ConformityEntityAttribute]
        public string ConformityId { get; set; }

        public bool ConformityIsAccepted { get; set; }

        public bool ConformityIsValid { get; set; }

        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public bool SupplierConfirmed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleConformityType, ConformityTypeConformitySupplierModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(c => c.ConformityType.Id))
                .ForMember(
                x => x.Description,
                opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(
                x => x.ConformityId,
                opt => opt.MapFrom(x => x.Conformity.Id))
                .ForMember(
                x => x.ConformityIsAccepted,
                opt => opt.MapFrom(x => x.Conformity.IsAccepted))
                .ForMember(
                x => x.ConformityIsValid,
                opt => opt.MapFrom(x => x.Conformity.IsValid))
                .ForMember(
                x => x.SupplierId,
                opt => opt.MapFrom(x => x.Conformity.SupplierId))
                .ForMember(
                x => x.SupplierName,
                opt => opt.MapFrom(x => x.Conformity.Supplier.Name))
                ;
        }
    }
}
