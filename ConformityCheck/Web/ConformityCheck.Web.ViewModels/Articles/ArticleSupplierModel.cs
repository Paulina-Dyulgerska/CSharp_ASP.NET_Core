﻿namespace ConformityCheck.Web.ViewModels.Articles
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleSupplierModel : IMapFrom<ArticleSupplier>, IHaveCustomMappings
    {
        [SupplierEntityAttribute]
        public string Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsMainSupplier { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, ArticleSupplierModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(x => x.SupplierId))
                .ForMember(
                x => x.Number,
                opt => opt.MapFrom(x => x.Supplier.Number))
                .ForMember(
                x => x.Name,
                opt => opt.MapFrom(x => x.Supplier.Name))
                .ForMember(
                x => x.IsMainSupplier,
                opt => opt.MapFrom(x => x.IsMainSupplier));
        }
    }
}
