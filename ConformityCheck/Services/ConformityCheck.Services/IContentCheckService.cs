namespace ConformityCheck.Services
{
    using System.Threading.Tasks;

    public interface IContentCheckService
    {
        bool ArticleEntityIdCheck(string id);

        bool ArticleSupplierEntityIdCheck(string articleId, string supplierId);

        bool ArticleConformityTypeEntityIdCheck(string articleId, int conformityTypeId);

        bool ArticleEntityNumberCheck(string input);

        bool ConformityTypeEntityIdCheck(int id);

        bool ConformityTypeEntityDescriptionCheck(string input);

        bool ConformityTypeArticlesCheck(int id);

        bool ConformityTypeProductsCheck(int id);

        bool ConformityTypeConformitiesCheck(int id);

        bool ProductEntityIdCheck(string id);

        bool SubstanceEntityIdCheck(int id);

        bool SupplierEntityIdCheck(string id);

        bool SupplierEntityNameCheck(string input);

        bool SupplierEntityNumberCheck(string input);

        bool ConformityEntityIdCheck(string id);
    }
}
