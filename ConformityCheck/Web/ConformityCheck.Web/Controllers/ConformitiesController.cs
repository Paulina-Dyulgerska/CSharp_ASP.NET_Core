namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Messaging;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    // [AutoValidateAntiforgeryToken] - it is globaly as a service and is in the DC.
    public class ConformitiesController : BaseController
    {
        private const string ArticlesCallerViewName = GlobalConstants.Articles;
        private const string SuppliersCallerViewName = GlobalConstants.Suppliers;
        private const string ConformitiesCallerViewName = GlobalConstants.Conformities;
        private readonly string conformityFilesDirectory;
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly IConformitiesService conformitiesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender emailSender;
        private readonly ILogger<ConformitiesController> logger;

        public ConformitiesController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            IConformitiesService conformitiesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            ILogger<ConformitiesController> logger)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.conformitiesService = conformitiesService;
            this.userManager = userManager;
            this.environment = environment;
            this.emailSender = emailSender;
            this.logger = logger;
            this.conformityFilesDirectory = $"{this.environment.WebRootPath}/files";
        }

        public async Task<IActionResult> Index(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new ConformitiesIndexModel
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
                    model.ItemsCount = this.conformitiesService.GetCount();
                    model.Conformities = await this.conformitiesService
                                                .GetAllOrderedAsPagesAsync<ConformityExportModel>(
                                                    input.CurrentSortOrder,
                                                    input.PageNumber,
                                                    input.ItemsPerPage);
                }
                else
                {
                    input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                    model.ItemsCount = this.conformitiesService.GetCountBySearchInput(input.CurrentSearchInput);
                    model.Conformities = await this.conformitiesService
                                                .GetAllBySearchInputOrderedAsPagesAsync<ConformityExportModel>(
                                                    input.CurrentSearchInput,
                                                    input.CurrentSortOrder,
                                                    input.PageNumber,
                                                    input.ItemsPerPage);
                }

                this.logger.LogInformation($"Conformities loaded successfully");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformities loading failed: {ex}");

                return this.Redirect("/");
            }

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        // [ValidateAntiforgeryToken] - it is globaly as a service
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ConformityCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.ValidForSingleArticle = false;

                return this.View();
            }

            // TODO - add just supplier's conformity - not connected with articles but a general one.
            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformitiesService.CreateAsync(input, user.Id, this.conformityFilesDirectory);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityCreatedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity for article: {input.ArticleId}, conformity type: {input.ConformityTypeId} and supplier: {input.SupplierId} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity creation failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> AddToArticleConformityType(ArticleIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index), ArticlesCallerViewName);
            }

            // if error accure here, the Home/Error page will be displayed
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesExportModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToArticleConformityType(ArticleManageConformitiesInputModel input)
        {
            var id = input.Conformity.ArticleId;

            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesExportModel>(id);

                return this.View(model);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformitiesService.CreateAsync(input.Conformity, user.Id, this.conformityFilesDirectory);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityCreatedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity for article: {input.Conformity.ArticleId}, conformity type: {input.Conformity.ConformityTypeId} and supplier: {input.Conformity.SupplierId} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogInformation($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity creation failed: {ex}");

                var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesExportModel>(id);

                return this.View(model);
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id });
        }

        [Authorize]
        public async Task<IActionResult> AddToArticleSupplierConformityType(ConformityGetInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index), input.CallerViewName);
            }

            var model = await this.conformitiesService.GetForCreateAsync(input);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToArticleSupplierConformityType(ConformityCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.ValidForSingleArticle = false;

                var model = await this.conformitiesService.GetForCreateAsync(new ConformityGetInputModel
                {
                    ArticleId = input.ArticleId,
                    ConformityTypeId = input.ConformityTypeId,
                    SupplierId = input.SupplierId,
                    CallerViewName = input.CallerViewName,
                });

                return this.View(model);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformitiesService.CreateAsync(input, user.Id, this.conformityFilesDirectory);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityCreatedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity for article: {input.ArticleId}, conformity type: {input.ConformityTypeId} and supplier: {input.SupplierId} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogInformation($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity creation failed: {ex}");

                return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
            }

            // TODO return this.RedirectToAction("Details", CallerViewName, new { id = input.CallerId });
            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            if (input.CallerViewName == ConformitiesCallerViewName)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }

        [Authorize]
        public async Task<IActionResult> Details(ConformityIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            var model = await this.conformitiesService.GetByIdAsync<ConformityExportModel>(input.Id);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(ConformityIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            var model = await this.conformitiesService.GetByIdAsync<ConformityEditInputModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ConformityEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.conformitiesService.GetByIdAsync<ConformityEditInputModel>(input.Id);

                return this.View(model);
            }

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformitiesService.EditAsync(input, user.Id, this.conformityFilesDirectory);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityEditedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity: {input.Id} was edited by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogInformation($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity modification failed: {ex}");

                var model = await this.conformitiesService.GetByIdAsync<ConformityEditInputModel>(input.Id);

                return this.View(model);
            }

            // TODO return this.RedirectToAction("Details", CallerViewName, new { id = input.CallerId });
            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            if (input.CallerViewName == ConformitiesCallerViewName)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(ConformityDeleteInputModel input)
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
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);
                await this.conformitiesService.DeleteAsync(input.Id, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ConformityDeletedSuccessfullyMessage;
                this.logger.LogInformation($"Conformity: {input.Id} was deleted by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogInformation($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity deletion failed: {ex}");

                // if error accure here, the Home/Error page will be displayed
            }

            // TODO return this.RedirectToAction("Details", CallerViewName, new { id = input.CallerId });
            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            if (input.CallerViewName == ConformitiesCallerViewName)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }

        // TODO - delete - not used:
        [Authorize]
        public IActionResult PdfPartial(string conformityFileUrl)
        {
            return this.PartialView("_PartialDocumentPreview", conformityFileUrl);
        }
    }
}
