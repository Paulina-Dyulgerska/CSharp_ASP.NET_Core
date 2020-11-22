namespace ConformityCheck.Web.ViewComponents
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsViewComponent : ViewComponent
    {
        private readonly IContentDeliveryService contentDeliveryService;

        public ProductsViewComponent(IContentDeliveryService contentDeliveryService)
        {
            this.contentDeliveryService = contentDeliveryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await this.contentDeliveryService.GetAllProductsAsync();

            return this.View(viewModel);
        }
    }
}
