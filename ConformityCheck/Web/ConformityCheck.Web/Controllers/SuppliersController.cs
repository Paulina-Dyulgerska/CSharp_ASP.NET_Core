namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class SuppliersController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<SuppliersController> logger;

        public SuppliersController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            UserManager<ApplicationUser> userManager,
            ILogger<SuppliersController> logger)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.userManager = userManager;
            this.logger = logger;
        }

        // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public async Task<IActionResult> Index(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            // var model = await this.suppliersService.GetAllAsNoTrackingOrderedAsync<SupplierExportModel>();
            var model = new SuppliersIndexModel
            {
                ItemsPerPage = input.ItemsPerPage,
                PageNumber = input.PageNumber,
                PagingControllerActionCallName = nameof(this.Index),
                CurrentSortOrder = input.CurrentSortOrder,
                CurrentSearchInput = input.CurrentSearchInput,
                CurrentSortDirection = input.CurrentSortDirection == GlobalConstants.CurrentSortDirectionDesc ?
                                    GlobalConstants.CurrentSortDirectionAsc : GlobalConstants.CurrentSortDirectionDesc,
            };

            try
            {
                if (string.IsNullOrWhiteSpace(input.CurrentSearchInput))
                {
                    model.ItemsCount = this.suppliersService.GetCount();
                    model.Suppliers = await this.suppliersService
                                                .GetAllOrderedAsPagesAsync<SupplierExportModel>(
                                                    input.CurrentSortOrder,
                                                    input.PageNumber,
                                                    input.ItemsPerPage);
                }
                else
                {
                    input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                    model.ItemsCount = this.suppliersService.GetCountBySearchInput(input.CurrentSearchInput);
                    model.Suppliers = await this.suppliersService
                                                .GetAllBySearchInputOrderedAsPagesAsync<SupplierExportModel>(
                                                input.CurrentSearchInput,
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
                }

                this.logger.LogInformation($"Suppliers loaded successfully");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Suppliers loading failed: {ex}");

                return this.Redirect("/");
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
                return this.View(input);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.suppliersService.CreateAsync(input, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.SupplierCreatedSuccessfullyMessage;
                this.logger.LogInformation($"Supplier number: {input.Number} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Supplier creation failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(SupplierIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            var model = await this.suppliersService.GetByIdAsync<SupplierEditInputModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(SupplierEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.suppliersService.EditAsync(input, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.SupplierEditedSuccessfullyMessage;
                this.logger.LogInformation($"Supplier number: {input.Id} was edited by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Supplier modification failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> Details(SupplierArticlesDetailsInputModel input)
        {
            // var model = await this.suppliersService.DetailsByIdAsync(id);
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            try
            {
                // var model = await this.suppliersService.DetailsBySupplierAsync(input);
                var model = await this.suppliersService.GetByIdWIthArticlesAndConformityOrderedAsPageAsync(
                                                input.Id,
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
                model.ItemsPerPage = input.ItemsPerPage;
                model.PageNumber = input.PageNumber;
                model.PagingControllerActionCallName = nameof(this.Details);
                model.CurrentSortOrder = input.CurrentSortOrder;
                model.CurrentSearchInput = input.CurrentSearchInput;
                model.CurrentSortDirection = input.CurrentSortDirection == GlobalConstants.CurrentSortDirectionDesc ?
                                        GlobalConstants.CurrentSortDirectionAsc : GlobalConstants.CurrentSortDirectionDesc;
                return this.View(model);
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Supplier details loading failed: {ex}");

                // return user to the List all view instead of Home/Error page
                return this.RedirectToAction(nameof(this.Index));
            }
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(SupplierIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.suppliersService.DeleteAsync(input.Id, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.SupplierDeletedSuccessfullyMessage;
                this.logger.LogInformation($"Supplier: {input.Id} was deleted by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Supplier deletion failed: {ex}");

                // if error accure here, the Home/Error page will be displayed
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
