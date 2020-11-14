namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;

    public interface IArticlesService : IService
    {
        Article GetArticle(string articleId);

        Supplier GetSupplier(string supplierId);

        Task CreateAsync(CreateArticleInputModel articleViewModel);

        Task<int> DeleteArticleAsync(string articleId);

        void DeleteSupplierFromArticle(string articleId, string supplierId);

        IEnumerable<string> GetSuppliersNumbersList(string articleId);

        int GetSuppliersCount(string articleId);

        IEnumerable<SupplierExportDTO> ListArticleSuppliers(string articleId);

        IEnumerable<ConformityImportDTO> ListArticleConformities(string articleId);

        IEnumerable<ProductDTO> ListArticleProducts(string articleId);

        void UpdateArticle(CreateArticleInputModel articleViewModel);

        void AddConformityToArticle(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        void DeleteConformity(string articleId);

        IEnumerable<ArticleExportDTO> SearchByArticleNumber(string artileId); //part of the number

        IEnumerable<ArticleExportDTO> SearchBySupplierNumber(string supplierNumber);

        IEnumerable<ArticleExportDTO> SearchByConformityType(string conformityType);

        IEnumerable<ArticleExportDTO> SearchByConfirmedStatus(string status); //confirmed or not
    }
}
