namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class ConformityTypesController : Controller
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ConformityTypesController> logger;

        public ConformityTypesController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            UserManager<ApplicationUser> userManager,
            ILogger<ConformityTypesController> logger)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            // var model = await this.conformityTypesService.GetAllAsNoTrackingOrderedAsync<ConformityTypeExportModel>();
            var model = new ConformityTypesIndexModel
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
                    model.ItemsCount = this.conformityTypesService.GetCount();
                    model.ConformityTypes = await this.conformityTypesService
                                                .GetAllOrderedAsPagesAsync<ConformityTypeExportModel>(
                                                    input.CurrentSortOrder,
                                                    input.PageNumber,
                                                    input.ItemsPerPage);
                }
                else
                {
                    input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                    model.ItemsCount = this.conformityTypesService.GetCountBySearchInput(input.CurrentSearchInput);
                    model.ConformityTypes = await this.conformityTypesService
                                                .GetAllBySearchInputOrderedAsPagesAsync<ConformityTypeExportModel>(
                                                input.CurrentSearchInput,
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
                }

                this.logger.LogInformation($"Conformity types loaded successfully");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity types loading failed: {ex}");

                return this.Redirect("/");
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

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformityTypesService.CreateAsync(input, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityTypeCreatedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity type: {input.Description} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity type creation failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(ConformityTypeIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            var model = await this.conformityTypesService.GetByIdAsync<ConformityTypeEditInputModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ConformityTypeEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.conformityTypesService.GetByIdAsync<ConformityTypeEditInputModel>(input.Id);

                // this is needed to reject the creation of conformity type with a dublicated description.
                // not needed since the validation attribute is created.
                var propertyDescription = nameof(input.Description);

                if (this.ModelState.Keys.Contains(propertyDescription)
                    && this.ModelState[propertyDescription].AttemptedValue.ToString() == model.Description)
                {
                    this.ModelState.Remove(propertyDescription);
                    return await this.Edit(input);
                }

                return this.View(model);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformityTypesService.EditAsync(input, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityTypeEditedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity type: {input.Id} was edited by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity type modification failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            // TODO: return this.RedirectToAction(nameof(this.Details), new { input.Id });
            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(ConformityTypeDeleteInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] += error.ErrorMessage + Environment.NewLine;
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformityTypesService.DeleteAsync(input.Id, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityTypeDeletedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity type: {input.Id} was deleted by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity type deletion failed: {ex}");

                // if error accure here, the Home/Error page will be displayed
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
