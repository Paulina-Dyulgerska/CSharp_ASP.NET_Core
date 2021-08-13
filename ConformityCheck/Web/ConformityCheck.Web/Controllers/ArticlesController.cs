namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(
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
        public async Task<IActionResult> ListAll(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new ArticlesListAllModel
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
                model.ItemsCount = this.articlesService.GetCount();
                model.Articles = await this.articlesService
                                            .GetAllOrderedAsPagesAsync<ArticleDetailsExportModel>(
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
            }
            else
            {
                input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                model.ItemsCount = this.articlesService.GetCountBySearchInput(input.CurrentSearchInput);
                model.Articles = await this.articlesService
                                            .GetAllBySearchInputOrderedAsPagesAsync<ArticleDetailsExportModel>(
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
        public async Task<IActionResult> Create(ArticleCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.articlesService.CreateAsync(input, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleCreatedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Details(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(input.Id);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ArticleEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(input.Id);

                return this.View(model);

                //// not needed with the current model
                // var propertyNumber = nameof(input.Number);
                // if (this.ModelState.Keys.Contains(propertyNumber)
                //    && this.ModelState[propertyNumber].AttemptedValue.ToString() == model.Number)
                // {
                //    this.ModelState.Remove(propertyNumber);
                //    return await this.Edit(input);
                // }
                // return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.articlesService.EditAsync(input, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> AddSupplier(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.AddSupplierAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> ChangeMainSupplier(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeMainSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.ChangeMainSupplierAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveSupplier(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.RemoveSupplierAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> AddConformityType(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesExportModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.AddConformityTypeAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveConformityType(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesExportModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.RemoveConformityTypesAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveConformity(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(input.Id);

            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] =
                    GlobalConstants.ArticleInvalidId;
                return this.RedirectToAction(nameof(this.ListAll));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.articlesService.DeleteAsync(input.Id, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleDeletedSuccessfullyMessage;

            return this.RedirectToAction(nameof(this.ListAll));
        }
    }
}
