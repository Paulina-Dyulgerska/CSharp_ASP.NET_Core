namespace ConformityCheck.Services
{
    public interface IContentCheckService
    {
        bool ArticleEntityCheck(string id);

        bool ArticleSupplierEntityCheck(string articleId, string supplierId);

        bool ConformityTypeEntityCheck(string id);

        bool ConformityTypeEntityCheck(int id);

        bool ProductEntityCheck(string id);

        bool SubstanceEntityCheck(int id);

        bool SupplierEntityCheck(string id);
    }
}
