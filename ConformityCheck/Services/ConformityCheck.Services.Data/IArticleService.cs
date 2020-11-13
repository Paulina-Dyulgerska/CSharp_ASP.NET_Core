namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;

    public interface IArticleService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();

        Article GetArticle(string articleId);

        Supplier GetSupplier(string supplierId);

        Task CreateAsync(ArticleImportDTO articleImportDTO);

        Task<int> AddSupplierToArticleAsync(Article article, ArticleImportDTO articleImportDTO);

        Task<Supplier> GetOrCreateSupplierAsync(ArticleImportDTO articleImportDTO);

        Task<int> DeleteArticleAsync(string articleId);

        void DeleteSupplierFromArticle(string articleId, string supplierId);

        IEnumerable<string> GetSuppliersNumbersList(string articleId);

        int GetSuppliersCount(string articleId);

        IEnumerable<SupplierExportDTO> ListArticleSuppliers(string articleId);

        IEnumerable<ConformityImportDTO> ListArticleConformities(string articleId);

        IEnumerable<ProductDTO> ListArticleProducts(string articleId);

        void UpdateArticle(ArticleImportDTO articleImportDTO);

        void AddConformityToArticle(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        void DeleteConformity(string articleId);

        IEnumerable<ArticleExportDTO> SearchByArticleNumber(string artileId); //part of the number

        IEnumerable<ArticleExportDTO> SearchBySupplierNumber(string supplierNumber);

        IEnumerable<ArticleExportDTO> SearchByConformityType(string conformityType);

        IEnumerable<ArticleExportDTO> SearchByConfirmedStatus(string status); //confirmed or not
    }
}
