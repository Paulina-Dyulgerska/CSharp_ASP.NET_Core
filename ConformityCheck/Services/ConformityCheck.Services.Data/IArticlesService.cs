namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IArticlesService : IService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingFullInfoAsync<T>();

        Task<T> GetByIdAsync<T>(string id);

        Task<Article> GetByIdAsync(string id);

        Task CreateAsync(ArticleCreateModel input);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task RemoveConformityTypesAsync(ArticleManageConformityTypesModel input);

        Task AddConformityTypesAsync(ArticleManageConformityTypesModel input);

        Task AddSupplierAsync(Article article, string supplierId);

        Task RemoveSupplierAsync(ArticleManageSuppliersModel input);

        Task<int> DeleteAsync(string id);

        Task AddConformityAsync(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        Task DeleteConformityAsync(string id);

        Task EditAsync(ArticleEditModel input);

        Task ChangeMainSupplierAsync(ArticleManageSuppliersModel input);
    }
}
