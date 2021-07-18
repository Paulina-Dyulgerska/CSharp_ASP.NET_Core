namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(
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

        // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //[Authorize]
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
                CreatedOnSortParm = string.IsNullOrEmpty(input.CurrentSortOrder) ? "createdOn" : string.Empty,
                NumberSortParm = input.CurrentSortOrder == "numberDesc" ? "number" : "numberDesc",
                DescriptionSortParm = input.CurrentSortOrder == "descriptionDesc" ? "description" : "descriptionDesc",
                MainSupplierNameSortParm = input.CurrentSortOrder == "mainSupplierNameDesc" ? "mainSupplierName" : "mainSupplierNameDesc",
                MainSupplierNumberSortParm = input.CurrentSortOrder == "mainSupplierNumberDesc" ? "mainSupplierNumber" : "mainSupplierNumberDesc",
                MainSupplierAllConfirmedSortParm = input.CurrentSortOrder == "mainSupplierAllConfirmedDesc" ? "mainSupplierAllConfirmed" : "mainSupplierAllConfirmedDesc",
                AllSuppliersAllConfirmedSortParm = input.CurrentSortOrder == "allSuppliersAllConfirmedDesc" ? "allSuppliersAllConfirmed" : "allSuppliersAllConfirmedDesc",
                CurrentSortOrder = input.CurrentSortOrder,
                CurrentSearchInput = input.CurrentSearchInput,
                CurrentSortDirection = input.CurrentSortDirection == "sortDesc" ? "sortAsc" : "sortDesc",
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

        //[Authorize]
        //public async Task<IActionResult> ListAllUnconfirmedByMainSupplierArticles()
        //{
        //    var model = this.articlesService.
        //    return this.View(nameof(ListAll), model);
        //}

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

            // return this.Json(input);
            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            //trqbwa li da checkwam v DB za wsqko edno id, dali go imam v bazata?

            var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ArticleEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(input.Id);

                return this.View(model);

                //// not needed at the model with the current model
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
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> AddSupplier(string id)
        {
            var model = new ArticleManageSuppliersExportModel();
            try
            {
                model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(id);

            }
            catch (System.Exception)
            {

            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(input.Id);

                // if false article id is sent:
                // if (model == null)
                // {
                //     return this.RedirectToAction("Index", "Home");
                // }
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.articlesService.AddSupplierAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Edit), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> ChangeMainSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(id);

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
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersExportModel>(id);

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
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> AddConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.AddConformityTypeAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(input.Id);

                return this.View(model);
            }

            await this.articlesService.RemoveConformityTypesAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleEditedsuccessfullyMessage;

            return this.RedirectToAction(nameof(this.Details), new { input.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.articlesService.DeleteAsync(id, user.Id);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.ArticleDeletedsuccessfullyMessage;

            return this.View();
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleDetailsExportModel>(id);

            return this.View(model);
        }

        // [Authorize]
        public async Task<IActionResult> GetSuppliersById(string id)
        {
            var model = await this.articlesService.GetSuppliersByIdAsync<SupplierExportModel>(id);

            return this.Json(model);
        }

        // [Authorize]
        public async Task<IActionResult> GetConformityTypesByIdAndSupplier(
            string articleId,
            string supplierId)
        {
            var model = await this.articlesService
                .GetConformityTypesByIdAndSupplierAsync(articleId, supplierId);

            return this.Json(model);
        }

        // [Authorize]
        public async Task<IActionResult> GetByNumberOrDescription(string input)
        {
            var model = await this.articlesService.GetAllBySearchInputAsync<ArticleExportModel>(input);

            return this.Json(model);
        }

        // TODO - in a new controller ContentDeliveryController Api one:
        //[ApiController]
        //[Route("api/[controller]")]
        //public class VotesController : BaseController
        //{
        //    private readonly IVotesService votesService;

        //    public VotesController(IVotesService votesService)
        //    {
        //        this.votesService = votesService;
        //    }

        //    [HttpPost]
        //    [Authorize]
        //    public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteInputModel input)
        //    {
        //        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        await this.votesService.SetVoteAsync(input.RecipeId, userId, input.Value);
        //        var averageVotes = this.votesService.GetAverageVotes(input.RecipeId);
        //        return new PostVoteResponseModel { AverageVote = averageVotes };
        //    }
        //}
        //<form method = "post" id="antiForgeryForm"></form>
        //        @section Scripts
        //        {
        //    <script>
        //        $("li[data-vote]").each(function (el) {
        //            $(this).click(function() {
        //                var value = $(this).attr("data-vote");
        //                var recipeId = @Model.Id;
        //                var antiForgeryToken = $('#antiForgeryForm input[name=__RequestVerificationToken]').val();
        //                var data = { recipeId: recipeId, value: value };
        //                $.ajax({
        //            type: "POST",
        //                    url: "/api/Votes",
        //                    data: JSON.stringify(data),
        //                    headers:
        //                {
        //                    'X-CSRF-TOKEN': antiForgeryToken
        //                    },
        //                    success: function(data) {
        //                        $('#averageVoteValue').html(data.averageVote.toFixed(1));
        //                },
        //                    contentType: 'application/json',
        //                });
        //        })
        //        });
        //    </script>
        //}
    }
}
