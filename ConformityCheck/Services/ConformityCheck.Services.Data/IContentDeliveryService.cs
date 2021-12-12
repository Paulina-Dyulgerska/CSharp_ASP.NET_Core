namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContentDeliveryService
    {
        Task<IEnumerable<T>> GetAllArticlesAsync<T>();

        Task<IEnumerable<T>> GetLastCreatedArticlesAsync<T>();

        Task<IEnumerable<T>> GetAllSuppliersAsync<T>();

        Task<IEnumerable<T>> GetLastCreatedSuppliersAsync<T>();

        Task<IEnumerable<T>> GetAllConformityTypesAsync<T>();

        Task<IEnumerable<T>> GetLastCreatedConformityTypesAsync<T>();

        Task<IEnumerable<T>> GetAllProductsAsync<T>();

        Task<IEnumerable<T>> GetLastCreatedProductsAsync<T>();

        Task<IEnumerable<T>> GetAllRolesAsync<T>();

        Task<IEnumerable<T>> GetAllSubstancesAsync<T>();

        Task<IEnumerable<T>> GetLastCreatedSubstancesAsync<T>();
    }
}
