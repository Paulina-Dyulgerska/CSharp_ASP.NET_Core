namespace ConformityCheck.Web.Controllers
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.AspNetCore.Mvc;

    public class ConformitiesController : BaseController
    {
        private const string SuppliersCallerViewName = "Suppliers";
        private const string ArticlesCallerViewName = "Articles";
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;
        private readonly IConformitiesService conformitiesService;

        public ConformitiesController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            IConformitiesService conformitiesService)
        {
            this.articlesService = articlesService;
            this.supplierService = suppliersService;
            this.productService = productsService;
            this.conformityTypeService = conformityTypesService;
            this.substanceService = substancesService;
            this.conformitiesService = conformitiesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConformityCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.ValidForSingleArticle = false;

                return this.View(input);
            }

            await this.conformitiesService.CreateAsync(input);

            // return this.Json(input);
            return this.RedirectToAction("ListAll", "Articles");
        }

        public async Task<IActionResult> AddToArticleConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToArticleConformityType(ArticleManageConformitiesInputModel input)
        {
            var id = input.Conformity.ArticleId;

            if (!this.ModelState.IsValid)
            {
                var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesModel>(id);

                return this.View(model);
            }

            await this.conformitiesService.CreateAsync(input.Conformity);

            return this.RedirectToAction(nameof(ArticlesController.Edit), "Articles", new { id });
        }

        public async Task<IActionResult> Edit(ConformityEditGetModel input)
        {
            var model = await this.conformitiesService.GetForEditAsync(input);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConformityEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                var getModel = new ConformityEditGetModel
                {
                    ArticleId = input.ArticleId,
                    SupplierId = input.SupplierId,
                    ConformityTypeId = input.ConformityTypeId,
                    Id = input.Id,
                };

                var model = await this.conformitiesService.GetForEditAsync(getModel);

                return this.View(model);
            }

            await this.conformitiesService.EditAsync(input);

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }
    }
}
