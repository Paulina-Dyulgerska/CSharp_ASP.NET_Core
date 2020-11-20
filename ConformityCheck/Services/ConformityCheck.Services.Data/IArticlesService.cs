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

        Task CreateAsync(CreateArticleInputModel articleInputModel);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task AddSupplierAsync(Article article, string supplierId, string mainSupplierId = null);

        Task RemoveSupplierAsync(string articleId, string supplierId);

        Task<int> DeleteAsync(string articleId);

        Task AddConformityAsync(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        Task DeleteConformityAsync(string articleId);

        Task<EditExportModel> GetEditAsync(string articleId);

        Task PostEditAsync(EditExportModel articleInputModel);
    }
}
