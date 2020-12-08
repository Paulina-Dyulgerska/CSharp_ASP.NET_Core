namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.AspNetCore.Mvc;

    public class ConformitiesController : BaseController
    {
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
                //input.ValidForSingleArticle = false;
                //return this.RedirectToAction(nameof(this.Create));
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
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.conformitiesService.CreateAsync(input.Conformity);
            var id = input.Conformity.ArticleId;

            //return this.RedirectToAction("Edit", "Articles", new { input.Conformity.ArticleId });
            return this.RedirectToAction(nameof(ArticlesController.Edit), "Articles", new { id });
        }

        public async Task<IActionResult> Edit(ConformityEditBaseModel input)
        {
            var model = await this.conformitiesService.GetForEditAsync(input);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConformityEditModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.conformitiesService.EditAsync(input);
            var id = input.ArticleId;

            return this.RedirectToAction(nameof(ArticlesController.Details), "Articles", new { id });
        }
    }
}
