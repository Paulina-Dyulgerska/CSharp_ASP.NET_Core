using AutoMapper;
using ConformityCheck.Data.Models;
using ConformityCheck.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    public class ConformityTypeNumberExportModel : IMapFrom<ConformityType>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ConformityType, ConformityTypeNumberExportModel>().ForMember(
                x => x.Description, 
                opt => opt.MapFrom(x => x.Description));
        }
    }
}
