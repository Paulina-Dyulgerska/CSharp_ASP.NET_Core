namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ConformityTypesController : Controller
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;
        private readonly UserManager<ApplicationUser> userManager;

        public ConformityTypesController(
            IArticlesService articlesService,
            ISuppliersService supplierService,
            IProductsService productService,
            IConformityTypesService conformityTypeService,
            ISubstancesService substanceService,
            UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.supplierService = supplierService;
            this.productService = productService;
            this.conformityTypeService = conformityTypeService;
            this.substanceService = substanceService;
            this.userManager = userManager;
        }

        // public async Task<IActionResult> ListAll()
        // {
        //     var model = await this.conformityTypeService.GetAllAsNoTrackingOrderedAsync<ConformityTypeExportModel>();
        //     return this.View(model);
        // }
        public async Task<IActionResult> ListAll(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new ConformityTypesListAllModel
            {
                ItemsPerPage = input.ItemsPerPage,
                PageNumber = input.PageNumber,
                PagingControllerActionCallName = nameof(this.ListAll),
                CurrentSortOrder = input.CurrentSortOrder,
                CurrentSearchInput = input.CurrentSearchInput,
                CurrentSortDirection = input.CurrentSortDirection == GlobalConstants.CurrentSortDirectionDesc ?
                                    GlobalConstants.CurrentSortDirectionAsc : GlobalConstants.CurrentSortDirectionDesc,
            };

            if (string.IsNullOrWhiteSpace(input.CurrentSearchInput))
            {
                model.ItemsCount = this.conformityTypeService.GetCount();
                model.ConformityTypes = await this.conformityTypeService
                                            .GetAllOrderedAsPagesAsync<ConformityTypeExportModel>(
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
            }
            else
            {
                input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                model.ItemsCount = this.conformityTypeService.GetCountBySearchInput(input.CurrentSearchInput);
                model.ConformityTypes = await this.conformityTypeService
                                            .GetAllBySearchInputOrderedAsPagesAsync<ConformityTypeExportModel>(
                                            input.CurrentSearchInput,
                                            input.CurrentSortOrder,
                                            input.PageNumber,
                                            input.ItemsPerPage);
            }

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ConformityTypeCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformityTypeService.CreateAsync(input, userId);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityTypeCreatedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.conformityTypeService.GetByIdAsync<ConformityTypeEditInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ConformityTypeEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.conformityTypeService.GetByIdAsync<ConformityTypeEditInputModel>(input.Id);

                // this is needed to reject the creation of conformity type with a dublicated description.
                var propertyDescription = nameof(input.Description);

                if (this.ModelState.Keys.Contains(propertyDescription)
                    && this.ModelState[propertyDescription].AttemptedValue.ToString() == model.Description)
                {
                    this.ModelState.Remove(propertyDescription);

                    return await this.Edit(input);
                }

                return this.View(model);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformityTypeService.EditAsync(input, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityTypeEditedsuccessfullyMessage;

            // TODO: return this.RedirectToAction(nameof(this.Details), "ConformityTypes", new { input.Id });
            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Delete(ConformityTypeDeleteInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    this.TempData["ErrorMessage"] += error.ErrorMessage + Environment.NewLine;
                }

                return this.RedirectToAction(nameof(this.ListAll));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformityTypeService.DeleteAsync(input.Id, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityTypeDeletedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.ListAll));
        }

        // [Authorize]
        public async Task<IActionResult> GetByIdOrDescription(string input)
        {
            var model = await this.conformityTypeService.GetAllBySearchInputAsync<ConformityTypeExportModel>(input);

            return this.Json(model);
        }
    }
}
