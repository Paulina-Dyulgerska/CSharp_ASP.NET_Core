namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<TKey>
    {
        int GetCount();

        Task<T> GetByIdAsync<T>(TKey id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

    }
}
