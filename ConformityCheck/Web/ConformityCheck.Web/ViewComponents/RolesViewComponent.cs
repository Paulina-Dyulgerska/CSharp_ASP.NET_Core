﻿namespace ConformityCheck.Web.ViewComponents
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Administration.Roles.ViewComponents;
    using Microsoft.AspNetCore.Mvc;

    public class RolesViewComponent : ViewComponent
    {
        private readonly IContentDeliveryService contentDeliveryService;

        public RolesViewComponent(IContentDeliveryService contentDeliveryService)
        {
            this.contentDeliveryService = contentDeliveryService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await this.contentDeliveryService.GetAllRolesAsync<RolesViewComponentModel>();

            // the default view for this view components is called - this is the Default.cshtml!
            // Another View could be called, just put the name here, but IMPORTANT - do not forget to put this new view in the same
            // folder, that already contains the Default.cshtml view for this View component! This is the folder for this View component
            // views: ...\ConformityCheck\Web\ConformityCheck.Web\Views\Shared\Components\Roles\Default.cshtml
            // return this.View("Shared", viewModel);
            return this.View(viewModel);
        }
    }
}
