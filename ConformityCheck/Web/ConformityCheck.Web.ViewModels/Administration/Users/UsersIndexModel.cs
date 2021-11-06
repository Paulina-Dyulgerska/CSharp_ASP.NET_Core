namespace ConformityCheck.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class UsersIndexModel : PagingViewModel, IMapFrom<ApplicationUser>
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public string Id { get; set; }

        public string UserNameSortParam => this.CurrentSortOrder == GlobalConstants.UserNameSortParamDesc ?
            GlobalConstants.UserNameSortParam : GlobalConstants.UserNameSortParamDesc;

        public string UserEmailSortParam => this.CurrentSortOrder == GlobalConstants.UserEmailSortParamDesc ?
            GlobalConstants.UserEmailSortParam : GlobalConstants.UserEmailSortParamDesc;

        public string UserFullNameSortParam => this.CurrentSortOrder == GlobalConstants.UserFullNameSortParamDesc ?
            GlobalConstants.UserFullNameSortParam : GlobalConstants.UserFullNameSortParamDesc;

        public string UserEmailConfirmedSortParam => this.CurrentSortOrder == GlobalConstants.UserEmailConfirmedSortParamDesc ?
         GlobalConstants.UserEmailConfirmedSortParam : GlobalConstants.UserEmailConfirmedSortParamDesc;

        public string UserRolesSortParam => this.CurrentSortOrder == GlobalConstants.UserRolesSortParamDesc ?
            GlobalConstants.UserRolesSortParam : GlobalConstants.UserRolesSortParamDesc;
    }
}
