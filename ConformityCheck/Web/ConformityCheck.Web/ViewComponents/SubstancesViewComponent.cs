namespace ConformityCheck.Web.ViewComponents
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class SubstancesViewComponent : ViewComponent
    {
        private readonly IContentDeliveryService contentDeliveryService;

        public SubstancesViewComponent(IContentDeliveryService contentDeliveryService)
        {
            this.contentDeliveryService = contentDeliveryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await this.contentDeliveryService.GetAllSubstancesAsync();

            return this.View(viewModel);
        }
    }
}
