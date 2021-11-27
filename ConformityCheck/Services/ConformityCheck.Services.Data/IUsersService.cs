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

        UserViewModel GetById(string id);

        Task CreateAsync(UserCreateInputModel input, string userId);

        Task EditAsync(UserEditInputModel input, string userId);

        Task ManageRolesAsync(UserManageRolesInputModel input);

        Task<int> DeleteAsync(string id, string userId);
    }
}
