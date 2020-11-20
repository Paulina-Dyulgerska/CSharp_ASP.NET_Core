namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Articles.ViewComponents;
    using ConformityCheck.Web.ViewModels.ConformityTypes.ViewComponents;
    using ConformityCheck.Web.ViewModels.Products.ViewComponents;
    using ConformityCheck.Web.ViewModels.Substances.ViewComponents;
    using ConformityCheck.Web.ViewModels.Suppliers.ViewComponents;

    public interface IContentDeliveryService
    {
        Task<IEnumerable<ArticlesViewComponentModel>> GetAllArticlesAsync();

        Task<IEnumerable<ArticlesViewComponentModel>> GetLastCreatedArticlesAsync();

        Task<IEnumerable<SuppliersViewComponentModel>> GetAllSuppliersAsync();

        Task<IEnumerable<SuppliersViewComponentModel>> GetLastCreatedSuppliersAsync();

        Task<IEnumerable<ConformityTypesViewComponentModel>> GetAllConformityTypesAsync();

        Task<IEnumerable<ConformityTypesViewComponentModel>> GetLastCreatedConformityTypesAsync();

        Task<IEnumerable<ProductsViewComponentModel>> GetAllProductsAsync();

        Task<IEnumerable<ProductsViewComponentModel>> GetLstCreatedProductsAsync();

        Task<IEnumerable<SubstancesViewComponentModel>> GetAllSubstancesAsync();

        Task<IEnumerable<SubstancesViewComponentModel>> GetLastCreatedSubstancesAsync();
    }
}
