namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IArticlesService : IService<string>
    {
        Task<Article> GetByIdAsync(string id);

        Task CreateAsync(ArticleCreateModel input);

        Task EditAsync(ArticleEditInputModel input);

        Task<int> DeleteAsync(string id);

        Task AddSupplierAsync(Article article, string supplierId);

        Task ChangeMainSupplierAsync(ArticleManageSuppliersInputModel input);

        Task RemoveSupplierAsync(ArticleManageSuppliersInputModel input);

        Task AddConformityTypeAsync(ArticleManageConformityTypesInputModel input);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task RemoveConformityTypesAsync(ArticleManageConformityTypesInputModel input);

        Task AddConformityAsync(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        Task DeleteConformityAsync(string id);
    }
}
