namespace ConformityCheck.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName =>
            (string.IsNullOrEmpty(this.FirstName) && string.IsNullOrEmpty(this.LastName)) ?
                    null : $"{this.FirstName} {this.LastName}";

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UserRoleExportModel> Roles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(u => u.Roles.Select(r => new UserRoleExportModel
                {
                    Id = r.RoleId,
                })));
        }
    }
}
