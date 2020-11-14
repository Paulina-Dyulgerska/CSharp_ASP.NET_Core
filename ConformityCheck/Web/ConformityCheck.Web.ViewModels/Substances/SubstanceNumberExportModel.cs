using AutoMapper;
using ConformityCheck.Data.Models;
using ConformityCheck.Services.Mapping;

namespace ConformityCheck.Web.ViewModels.Substances
{
    public class SubstanceNumberExportModel : IMapFrom<Substance>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string CASNumber { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Substance, SubstanceNumberExportModel>();
        }
    }
}
