namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;

    public interface IArticlesService : IService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingFullInfoAsync<T>();

        Task<T> GetByIdAsync<T>(string articleId);

        Task<Article> GetByIdAsync(string articleId);

        Task CreateAsync(ArticleCreateModel articleInputModel);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task AddSupplierAsync(Article article, string supplierId);

        Task RemoveSupplierAsync(ArticleSuppliersModel input);

        Task<int> DeleteAsync(string articleId);

        Task AddConformityAsync(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        Task DeleteConformityAsync(string articleId);

        Task EditAsync(ArticleEditModel articleInputModel);

        Task ChangeMainSupplierAsync(ArticleChangeMainSupplierModel articleInputModel);
    }
}
