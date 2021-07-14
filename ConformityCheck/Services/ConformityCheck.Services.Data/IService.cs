namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<TKey>
    {
        int GetCount();

        int GetCountBySearchInput(string searchInput);

        Task<T> GetByIdAsync<T>(TKey id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput);

        Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(
                                                                        string searchInput,
                                                                        string sortOrder,
                                                                        int page,
                                                                        int itemsPerPage);

        Task<int> DeleteAsync(TKey id, string userId);
    }
}
