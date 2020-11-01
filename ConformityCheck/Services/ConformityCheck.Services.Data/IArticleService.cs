namespace ConformityCheck.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Models;

    public interface IArticleService
    {
        Article GetArticle(int articleId);

        Supplier GetSupplier(int supplierId);

        void Create(ArticleImportDTO articleImportDTO);

        void AddSupplierToArticle(Article article, ArticleImportDTO articleImportDTO);

        Supplier GetOrCreateSupplier(ArticleImportDTO articleImportDTO);

        Task<int> DeleteArticle(int articleId);

        void DeleteSupplierFromArticle(int articleId, int supplierId);

        IEnumerable<string> GetSuppliersNumbersList(int articleId);

        int GetSuppliersCount(int articleId);

        IEnumerable<SupplierExportDTO> ListArticleSuppliers(int articleId);

        IEnumerable<ConformityImportDTO> ListArticleConformities(int articleId);

        IEnumerable<ProductDTO> ListArticleProducts(int articleId);

        void UpdateArticle(ArticleImportDTO articleImportDTO);

        void AddConformityToArticle(int articleId, int supplierId, ArticleConformityImportDTO articleConformityImportDTO);

        void DeleteConformity(int articleId);

        IEnumerable<ArticleExportDTO> SearchByArticleNumber(int artileId); //part of the number

        IEnumerable<ArticleExportDTO> SearchBySupplierNumber(string supplierNumber);

        IEnumerable<ArticleExportDTO> SearchByConformityType(string conformityType);

        IEnumerable<ArticleExportDTO> SearchByConfirmedStatus(string status); //confirmed or not
    }
}
