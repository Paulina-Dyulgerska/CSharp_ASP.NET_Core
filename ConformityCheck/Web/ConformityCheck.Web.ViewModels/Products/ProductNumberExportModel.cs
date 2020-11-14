﻿using AutoMapper;
using ConformityCheck.Data.Models;
using ConformityCheck.Services.Mapping;

namespace ConformityCheck.Web.ViewModels.Products
{
    public class ProductNumberExportModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductNumberExportModel>();
        }
    }
}
