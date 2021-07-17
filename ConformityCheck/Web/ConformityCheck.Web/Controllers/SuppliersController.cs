namespace ConformityCheck.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.userManager = userManager;
        }

        // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // public async Task<IActionResult> ListAll()
        // {
        //     var model = await this.suppliersService.GetAllAsNoTrackingOrderedAsync<SupplierFullInfoModel>();
        //     return this.View(model);
        // }
        public async Task<IActionResult> ListAll(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new SuppliersListAllModel
            {
                ItemsPerPage = input.ItemsPerPage,
                PageNumber = input.PageNumber,
                PagingControllerActionCallName = nameof(this.ListAll),
                CreatedOnSortParm = string.IsNullOrEmpty(input.CurrentSortOrder) ? "createdOn" : string.Empty,
                NumberSortParm = input.CurrentSortOrder == "numberDesc" ? "number" : "numberDesc",
                NameSortParm = input.CurrentSortOrder == "nameDesc" ? "name" : "nameDesc",
                ArticlesCountSortParm = input.CurrentSortOrder == "articleCountDesc" ? "articleCount" : "articleCountDesc",
                UserEmailSortParm = input.CurrentSortOrder == "userEmailDesc" ? "userEmail" : "userEmailDesc",
                CurrentSortOrder = input.CurrentSortOrder,
                CurrentSearchInput = input.CurrentSearchInput,
                CurrentSortDirection = input.CurrentSortDirection == "sortDesc" ? "sortAsc" : "sortDesc",
            };

            if (string.IsNullOrWhiteSpace(input.CurrentSearchInput))
            {
                model.ItemsCount = this.suppliersService.GetCount();
                model.Suppliers = await this.suppliersService
                                            .GetAllOrderedAsPagesAsync<SupplierFullInfoModel>(
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
            }
            else
            {
                input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                model.ItemsCount = this.suppliersService.GetCountBySearchInput(input.CurrentSearchInput);
                model.Suppliers = await this.suppliersService
                                            .GetAllBySearchInputOrderedAsPagesAsync<SupplierFullInfoModel>(
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SupplierCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.suppliersService.CreateAsync(input, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.SupplierCreatedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.suppliersService.GetByIdAsync<SupplierEditInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(SupplierEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.suppliersService.EditAsync(input, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.SupplierEditedsuccessfullyMessage;

            // TODO: return this.RedirectToAction(nameof(this.Details), "Suppliers", new { input.Id });
            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            // var model = await this.suppliersService.GetByIdAsync<SupplierDetailsModel>(id);
            var model = await this.suppliersService.DetailsByIdAsync(id);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.suppliersService.DeleteAsync(id, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.SupplierDeletedsuccessfullyMessage;

            return this.View();
        }

        // [Authorize]
        public async Task<IActionResult> GetArticlesById(string id)
        {
            var model = await this.suppliersService.GetArticlesByIdAsync<ArticleExportModel>(id);

            return this.Json(model);
        }

        // [Authorize]
        public async Task<IActionResult> GetByNumberOrName(
            string input)
        {
            var model = await this.suppliersService.GetAllBySearchInputAsync<SupplierExportModel>(input);

            return this.Json(model);
        }
    }
}
