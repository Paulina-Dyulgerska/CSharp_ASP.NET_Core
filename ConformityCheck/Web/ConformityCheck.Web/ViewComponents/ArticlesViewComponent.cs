namespace ConformityCheck.Web.ViewComponents
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesViewComponent : ViewComponent
    {
        private readonly IContentDeliveryService contentDeliveryService;

        public ArticlesViewComponent(IContentDeliveryService contentDeliveryService)
        {
            this.contentDeliveryService = contentDeliveryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()

        {
            var viewModel = await this.contentDeliveryService.GetAllArticlesAsync();

            return this.View(viewModel);
            //return this.View("Shared",viewModel);
            //vika se po default Default.cshtml view-to!!! No moga da si vikna i drugo, prosto shte mu
            //napisha imeto tuk, 
            //vajnoto e towa novo view da se namira v syshtata smotana papka, v koqto se namira i Default.cshtml!!!
        }
    }
}
