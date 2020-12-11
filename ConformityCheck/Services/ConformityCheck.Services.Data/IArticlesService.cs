namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IArticlesService : IService<string>
    {
        //Task<Article> GetByIdAsync(string id); //to delete it

        Task CreateAsync(ArticleCreateInputModel input);

        Task EditAsync(ArticleEditInputModel input);

        Task<int> DeleteAsync(string id);

        Task AddSupplierAsync(ArticleManageSuppliersInputModel input);

        Task ChangeMainSupplierAsync(ArticleManageSuppliersInputModel input);

        Task RemoveSupplierAsync(ArticleManageSuppliersInputModel input);

        Task AddConformityTypeAsync(ArticleManageConformityTypesInputModel input);

        Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes);

        Task RemoveConformityTypesAsync(ArticleManageConformityTypesInputModel input);

        Task<IEnumerable<T>> GetSuppliersByIdAsync<T>(string id);

        // TODO - remove ConformityTypeModel - put T!!!
        Task<IEnumerable<ConformityTypeExportModel>> GetConformityTypesByIdAsync(
            string articleId,
            string supplierId);
    }
}
