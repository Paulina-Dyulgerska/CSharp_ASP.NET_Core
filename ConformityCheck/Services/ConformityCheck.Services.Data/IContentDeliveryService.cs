namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Articles.ViewComponents;
    using ConformityCheck.Web.ViewModels.ConformityTypes.ViewComponents;
    using ConformityCheck.Web.ViewModels.Suppliers.ViewComponents;

    public interface IContentDeliveryService
    {
        Task<IEnumerable<ArticleViewComponentModel>> GetAllArticlesAsync();

        Task<IEnumerable<ArticleViewComponentModel>> GetLastCreatedArticlesAsync();

        Task<IEnumerable<SupplierViewComponentModel>> GetAllSuppliersAsync();

        Task<IEnumerable<SupplierViewComponentModel>> GetLastCreatedSuppliersAsync();

        Task<IEnumerable<ConformityTypeViewComponentModel>> GetAllConformityTypesAsync();

        Task<IEnumerable<ConformityTypeViewComponentModel>> GetLastCreatedConformityTypesAsync();

    }
}
