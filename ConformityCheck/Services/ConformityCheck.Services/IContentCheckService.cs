namespace ConformityCheck.Services
{
    using System.Threading.Tasks;

    public interface IContentCheckService
    {
        bool ArticleEntityIdCheck(string id);

        bool ArticleSupplierEntityIdCheck(string articleId, string supplierId);

        bool ConformityTypeEntityIdCheck(int id);

        bool ConformityTypeEntityDescriptionCheck(string input);

        bool ProductEntityIdCheck(string id);

        bool SubstanceEntityIdCheck(int id);

        bool SupplierEntityIdCheck(string id);
    }
}
