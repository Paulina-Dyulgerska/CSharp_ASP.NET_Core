namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersService : IUsersService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this.roleManager = this.serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        }

        public int GetCount()
        {
            return this.userManager.Users.Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            var count = this.GetAll()
                .Where(x => x.UserName.Contains(searchInput)
                            || x.Email.Contains(searchInput)
                            || x.Roles.Select(r => r.Name).Contains(searchInput))
                .Count();

            return count;
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            var users = this.userManager.Users
                                    .To<UserViewModel>()
                                    .ToList();

            this.GetRoleNames(users);

            return users;
        }

        public IEnumerable<UserViewModel> GetAllBySearchInput(string searchInput)
        {
            var entities = this.GetAll()
                           .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                        || x.Email.ToLower().Contains(searchInput.ToLower())
                                        || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                           .ToList();

            return entities;
        }

        public IEnumerable<UserViewModel> GetAllBySearchInputOrderedAsPages(
            string searchInput,
            string sortOrder,
            int page,
            int itemsPerPage)
        {
            switch (sortOrder)
            {
                case GlobalConstants.UserNameSortParam:
                    var usersWithUserNameSortParam = this.GetAll()
                                .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                                .OrderBy(x => x.UserName)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .ToList();
                    return usersWithUserNameSortParam;

                case GlobalConstants.UserNameSortParamDesc:
                    var usersWithUserNameSortParamDesc = this.GetAll()
                                .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                                .OrderByDescending(x => x.UserName)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .ToList();
                    return usersWithUserNameSortParamDesc;

                case GlobalConstants.UserEmailSortParam:
                    var usersWithUserEmailSortParam = this.GetAll()
                                .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                                .OrderBy(x => x.Email)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .ToList();
                    return usersWithUserEmailSortParam;

                case GlobalConstants.UserEmailSortParamDesc:
                    var usersWithUserEmailSortParamDesc = this.GetAll()
                                .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                                .OrderByDescending(x => x.Email)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .ToList();
                    return usersWithUserEmailSortParamDesc;

                case GlobalConstants.UserFullNameSortParam:
                    var usersWithUserFullNameSortParam = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderBy(x => x.FullName)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserFullNameSortParam;

                case GlobalConstants.UserFullNameSortParamDesc:
                    var usersWithUserFullNameSortParamDesc = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderByDescending(x => x.FullName)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserFullNameSortParamDesc;

                case GlobalConstants.UserRolesSortParam:
                    var usersWithUserRolesSortParam = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderBy(x => string.Join(", ", x.Roles.Select(r => r.Name).ToList()))
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserRolesSortParam;

                case GlobalConstants.UserRolesSortParamDesc:
                    var usersWithUserRolesSortParamDesc = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderByDescending(x => string.Join(", ", x.Roles.Select(r => r.Name).ToList()))
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserRolesSortParamDesc;

                case GlobalConstants.UserEmailConfirmedSortParam:
                    var usersWithUserEmailConfirmedSortParam = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderBy(x => x.EmailConfirmed)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserEmailConfirmedSortParam;

                case GlobalConstants.UserEmailConfirmedSortParamDesc:
                    var usersWithUserEmailConfirmedSortParamDesc = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                                || x.Email.ToLower().Contains(searchInput.ToLower())
                                                || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderByDescending(x => x.EmailConfirmed)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithUserEmailConfirmedSortParamDesc;

                // case "createdOnDesc": show last created first
                default:
                    var usersWithCreatedOnSortParamDesc = this.GetAll()
                               .Where(x => x.UserName.ToLower().Contains(searchInput.ToLower())
                                            || x.Email.ToLower().Contains(searchInput.ToLower())
                                            || x.Roles.Any(x => x.Name.ToLower().Contains(searchInput.ToLower())))
                               .OrderByDescending(x => x.CreatedOn)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    return usersWithCreatedOnSortParamDesc;
            }
        }

        public IEnumerable<UserViewModel> GetAllOrderedAsPages(string sortOrder, int page, int itemsPerPage)
        {
            switch (sortOrder)
            {
                case GlobalConstants.UserNameSortParam:
                    var usersWithUserNameSortParam = this.userManager
                                .Users
                                .OrderBy(x => x.UserName)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<UserViewModel>()
                                .ToList();
                    this.GetRoleNames(usersWithUserNameSortParam);
                    return usersWithUserNameSortParam;

                case GlobalConstants.UserNameSortParamDesc:
                    var usersWithUserNameSortParamDesc = this.userManager
                                .Users
                                .OrderByDescending(x => x.UserName)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<UserViewModel>()
                                .ToList();
                    this.GetRoleNames(usersWithUserNameSortParamDesc);
                    return usersWithUserNameSortParamDesc;

                case GlobalConstants.UserEmailSortParam:
                    var usersWithUserEmailSortParam = this.userManager
                                .Users
                                .OrderBy(x => x.Email)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<UserViewModel>()
                                .ToList();
                    this.GetRoleNames(usersWithUserEmailSortParam);
                    return usersWithUserEmailSortParam;

                case GlobalConstants.UserEmailSortParamDesc:
                    var usersWithUserEmailSortParamDesc = this.userManager
                                .Users
                                .OrderByDescending(x => x.Email)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<UserViewModel>()
                                .ToList();
                    this.GetRoleNames(usersWithUserEmailSortParamDesc);
                    return usersWithUserEmailSortParamDesc;

                case GlobalConstants.UserFullNameSortParam:
                    var usersWithUserFullNameSortParam = this.userManager
                               .Users
                               .To<UserViewModel>()
                               .OrderBy(x => x.FullName)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    this.GetRoleNames(usersWithUserFullNameSortParam);
                    return usersWithUserFullNameSortParam;

                case GlobalConstants.UserFullNameSortParamDesc:
                    var usersWithUserFullNameSortParamDesc = this.userManager
                               .Users
                               .To<UserViewModel>()
                               .OrderByDescending(x => x.FullName)
                               .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                               .ToList();
                    this.GetRoleNames(usersWithUserFullNameSortParamDesc);
                    return usersWithUserFullNameSortParamDesc;

                case GlobalConstants.UserRolesSortParam:
                    var usersWithUserRolesSortParam = this.userManager
                               .Users
                               .To<UserViewModel>()
                               .ToList();
                    this.GetRoleNames(usersWithUserRolesSortParam);
                    return usersWithUserRolesSortParam
                        .OrderBy(x => string.Join(", ", x.Roles))
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToList();

                case GlobalConstants.UserRolesSortParamDesc:
                    var usersWithUserRolesSortParamDesc = this.userManager
                               .Users
                               .To<UserViewModel>()
                               .ToList();
                    this.GetRoleNames(usersWithUserRolesSortParamDesc);
                    return usersWithUserRolesSortParamDesc
                        .OrderByDescending(x => string.Join(", ", x.Roles))
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToList();

                case GlobalConstants.UserEmailConfirmedSortParam:
                    var usersWithUserEmailConfirmedSortParam = this.userManager
                                     .Users
                                     .To<UserViewModel>()
                                     .OrderBy(x => x.EmailConfirmed)
                                     .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                     .ToList();
                    this.GetRoleNames(usersWithUserEmailConfirmedSortParam);
                    return usersWithUserEmailConfirmedSortParam;

                case GlobalConstants.UserEmailConfirmedSortParamDesc:
                    var usersWithUserEmailConfirmedSortParamDesc = this.userManager
                                     .Users
                                     .To<UserViewModel>()
                                     .OrderByDescending(x => x.EmailConfirmed)
                                     .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                     .ToList();
                    this.GetRoleNames(usersWithUserEmailConfirmedSortParamDesc);
                    return usersWithUserEmailConfirmedSortParamDesc;

                // case "createdOnDesc": show last created first
                default:
                    var usersWithCreatedOnSortParamDesc = this.userManager
                                     .Users
                                     .To<UserViewModel>()
                                     .OrderByDescending(x => x.CreatedOn)
                                     .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                     .ToList();
                    this.GetRoleNames(usersWithCreatedOnSortParamDesc);
                    return usersWithCreatedOnSortParamDesc;
            }
        }

        public async Task<UserViewModel> GetById(string id)
        {
            var appllicationUser = await this.userManager.FindByIdAsync(id);
            var user = appllicationUser.To<UserViewModel>();
            var entities = new UserViewModel[1] { user };

            this.GetRoleNames(entities);

            return entities.FirstOrDefault();
        }

        public async Task CreateAsync(UserCreateInputModel input)
        {
            var user = new ApplicationUser
            {
                UserName = input.UserName,
                Email = input.Email,
                EmailConfirmed = input.EmailConfirmed,
            };
            var resultUserAdd = await this.userManager.CreateAsync(user, input.Password);

            if (!resultUserAdd.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, resultUserAdd.Errors.Select(e => e.Description)));
            }

            foreach (var roleId in input.Roles)
            {
                var role = await this.roleManager.FindByIdAsync(roleId);
                var resultUserToRoleAdd = await this.userManager.AddToRoleAsync(user, role.Name);

                if (!resultUserToRoleAdd.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, resultUserAdd.Errors.Select(e => e.Description)));
                }
            }
        }

        public Task EditAsync(UserEditInputModel input)
        {
            throw new NotImplementedException();
        }

        public Task ManageRolesAsync(UserManageRolesInputModel input)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        private void GetRoleNames(IEnumerable<UserViewModel> users)
        {
            var roles = this.roleManager.Roles.ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {
                    role.Name = roles.FirstOrDefault(x => x.Id == role.Id).Name;
                }
            }
        }
    }
}
