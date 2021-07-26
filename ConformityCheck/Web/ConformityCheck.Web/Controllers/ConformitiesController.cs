namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Claims;
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

    public class ConformitiesController : BaseController
    {
        private const string ArticlesCallerViewName = "Articles";
        private const string SuppliersCallerViewName = "Suppliers";
        private const string ConformitiesCallerViewName = "Conformities";
        private readonly string conformityFilesDirectory;
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;
        private readonly IConformitiesService conformitiesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender emailSender;

        public ConformitiesController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            IConformitiesService conformitiesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailSender emailSender)
        {
            this.articlesService = articlesService;
            this.supplierService = suppliersService;
            this.productService = productsService;
            this.conformityTypeService = conformityTypesService;
            this.substanceService = substancesService;
            this.conformitiesService = conformitiesService;
            this.userManager = userManager;
            this.environment = environment;
            this.emailSender = emailSender;
            this.conformityFilesDirectory = $"{this.environment.WebRootPath}/files";
        }

        public async Task<IActionResult> ListAll(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new ConformitiesListAllModel
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

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        // [AutoValidateAntiforgeryToken] - it is globaly as a service and is in the DC.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ConformityCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.ValidForSingleArticle = false;

                return this.View();
            }

            // TODO - add just supplier's conformity - not connected with articles but egeneral one.

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformitiesService.CreateAsync(input, user.Id, this.conformityFilesDirectory);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityCreatedSuccessfullyMessage;

            // return this.Json(input);
            // TODO: Redirect to conformity info page
            return this.RedirectToAction("ListAll", ArticlesCallerViewName);
        }

        //[Authorize]
        //public async Task<IActionResult> Details(string id)
        //{
        //    return this.View();
        //}

        [Authorize]
        public async Task<IActionResult> AddToArticleConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesExportModel>(id);

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

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.conformitiesService.CreateAsync(input.Conformity, user.Id, this.conformityFilesDirectory);
            }
            catch (Exception ex)
            {
                // da validiram s attribute i da ne prawq tezi gluposti tuk:!!!!!
                this.ModelState.AddModelError(string.Empty, ex.Message);

                return this.View(input);
            }

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id });
        }

        [Authorize]
        public async Task<IActionResult> AddToArticleSupplierConformityType(ConformityGetInputModel input)
        {
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

                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformitiesService.CreateAsync(input, user.Id, this.conformityFilesDirectory);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityEditedsuccessfullyMessage;

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }

        [Authorize]
        public async Task<IActionResult> Details(ConformityGetInputModel input)
        {
            var model = await this.conformitiesService.GetByIdAsync<ConformityExportModel>(input.Id);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(ConformityGetInputModel input)
        {
            var model = await this.conformitiesService.GetByIdAsync<ConformityEditInputModel>(input.Id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ConformityEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                var model = await this.conformitiesService.GetByIdAsync<ConformityEditInputModel>(input.Id);

                return this.View(model);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            // Not needed since the validation is not in the service but in with an attribute done:
            // try
            // {
            //     await this.conformitiesService.EditAsync(input, user.Id, this.conformityFilesDirectory);
            // }
            // catch (Exception ex)
            // {
            //     this.ModelState.AddModelError(string.Empty, ex.Message);
            //     //da validiram s attribute i da ne prawq tezi gluposti tuk:!!!!!
            //     var getModel = new ConformityGetInputModel
            //     {
            //         ArticleId = input.ArticleId,
            //         SupplierId = input.SupplierId,
            //         ConformityTypeId = input.ConformityTypeId,
            //         Id = input.Id,
            //     };
            //     var model = await this.conformitiesService.GetForEditAsync(getModel);
            //     return this.View(model);
            // }
            await this.conformitiesService.EditAsync(input, user.Id, this.conformityFilesDirectory);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityEditedsuccessfullyMessage;

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            if (input.CallerViewName == ConformitiesCallerViewName)
            {
                return this.RedirectToAction(nameof(ConformitiesController.ListAll), ConformitiesCallerViewName, new { id = input.Id });
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
            // TODO 
            // return this.RedirectToAction("Details", CallerViewName, new { id = input.CallerId });

        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(ConformityDeleteInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] +=
                        error.ErrorMessage + Environment.NewLine;
                }
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.conformitiesService.DeleteAsync(input.Id, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ConformityDeletedsuccessfullyMessage;

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Edit), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            return this.RedirectToAction(nameof(ArticlesController.Edit), ArticlesCallerViewName, new { id = input.ArticleId });
        }

        // TODO - delete - not used:
        [Authorize]
        public IActionResult PdfPartial(string conformityFileUrl)
        {
            return this.PartialView("_PartialDocumentPreview", conformityFileUrl);
        }

        // [HttpPost]
        [Authorize]
        public IActionResult ShowModalDocument(string conformityFileUrl)
        {
            string filePath = "~" + conformityFileUrl;
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = conformityFileUrl.Split('/').LastOrDefault(),
                Inline = true,
            };

            this.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            // Controller.File() works with [HttpGet]
            return this.File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        public async Task<IActionResult> GetByArticleOrSupplierOrConformityType(string input)
        {
            var model = await this.conformitiesService.GetAllBySearchInputAsync<ConformityExportModel>(input);

            return this.Json(model);
        }
    }
}
