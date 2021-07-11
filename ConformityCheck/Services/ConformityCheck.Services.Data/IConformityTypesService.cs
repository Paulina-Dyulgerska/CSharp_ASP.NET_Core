namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService<int>
    {
        int GetCountBySearchInput(string searchInput);

        Task CreateAsync(ConformityTypeCreateInputModel input, string userId);

        Task EditAsync(ConformityTypeEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<IEnumerable<T>> GetByIdOrDescriptionOrderedAsPagesAsync<T>(
                                                                        string searchInput,
                                                                        string sortOrder,
                                                                        int page,
                                                                        int itemsPerPage);

        Task<IEnumerable<T>> GetByIdOrDescriptionAsync<T>(string searchInput);
    }
}
