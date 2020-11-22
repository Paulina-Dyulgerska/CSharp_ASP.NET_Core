namespace ConformityCheck.Web.ViewComponents
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ConformityTypesViewComponent : ViewComponent
    {
        private readonly IContentDeliveryService contentDeliveryService;

        public ConformityTypesViewComponent(IContentDeliveryService contentDeliveryService)
        {
            this.contentDeliveryService = contentDeliveryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await this.contentDeliveryService.GetAllConformityTypesAsync();

            return this.View(viewModel);
        }
    }
}
