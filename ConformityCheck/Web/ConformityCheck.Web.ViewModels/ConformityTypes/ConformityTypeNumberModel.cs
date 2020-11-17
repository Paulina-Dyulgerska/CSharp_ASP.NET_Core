﻿namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeNumberModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}