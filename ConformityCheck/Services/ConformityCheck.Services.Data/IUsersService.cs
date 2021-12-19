namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        int GetCount();

        int GetCountBySearchInput(string searchInput);

        IEnumerable<UserViewModel> GetAll();

        IEnumerable<UserViewModel> GetAllOrderedAsPages(
            string sortOrder,
            int page,
            int itemsPerPage);

        IEnumerable<UserViewModel> GetAllBySearchInput(string searchInput);

        IEnumerable<UserViewModel> GetAllBySearchInputOrderedAsPages(
            string searchInput,
            string sortOrder,
            int page,
            int itemsPerPage);

        Task<UserViewModel> GetById(string id);

        Task CreateAsync(UserCreateInputModel input);

        Task EditAsync(UserEditInputModel input);

        Task ManageRolesAsync(UserManageRolesInputModel input);

        Task<int> DeleteAsync(string id);
    }
}
