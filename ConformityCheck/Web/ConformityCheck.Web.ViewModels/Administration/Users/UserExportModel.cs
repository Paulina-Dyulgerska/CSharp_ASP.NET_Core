namespace ConformityCheck.Web.ViewModels.Administration.Users
{
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class UserExportModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserExportModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(u => u.Roles.Select(r => r.RoleId)));
        }
    }
}
