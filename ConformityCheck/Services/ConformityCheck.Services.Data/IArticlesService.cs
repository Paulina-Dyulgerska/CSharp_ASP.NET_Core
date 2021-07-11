namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IArticlesService : IService<string>
    {
        // TODO - add this method: int GetCountUnconfirmedByMainSupplier();
        int GetCountBySearchInput(string searchInput);

        Task CreateAsync(ArticleCreateInputModel input, string userId);

        Task EditAsync(ArticleEditInputModel input, string userId);

        Task<int> DeleteAsync(string id, string userId);

        Task AddSupplierAsync(ArticleManageSuppliersInputModel input);

        Task ChangeMainSupplierAsync(ArticleManageSuppliersInputModel input);

        Task RemoveSupplierAsync(ArticleManageSuppliersInputModel input);

        Task AddConformityTypeAsync(ArticleManageConformityTypesInputModel input);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task RemoveConformityTypesAsync(ArticleManageConformityTypesInputModel input);

        Task<IEnumerable<T>> GetSuppliersByIdAsync<T>(string id);

        Task<IEnumerable<ConformityTypeExportModel>> GetConformityTypesByIdAndSupplierAsync(
            string articleId,
            string supplierId);

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<IEnumerable<T>> GetByNumberOrDescriptionOrderedAsPagesAsync<T>(
            string searchInput,
            string sortOrder,
            int page,
            int itemsPerPage);

        Task<IEnumerable<T>> GetByNumberOrDescriptionAsync<T>(string searchInput); // where T : ArticleDetailsModel;

        // TODO: add this method: Task<IEnumerable<T>> GetUnconfirmedByMainSupplierAsPagesAsync<T>(int page, int itemsPerPage);
    }
}
