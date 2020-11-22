namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;

        public ArticlesController(
            IArticlesService articlesService,
            ISuppliersService supplierService,
            IProductsService productService,
            IConformityTypesService conformityTypeService,
            ISubstancesService substanceService)
        {
            this.articlesService = articlesService;
            this.supplierService = supplierService;
            this.productService = productService;
            this.conformityTypeService = conformityTypeService;
            this.substanceService = substanceService;
        }

        public async Task<IActionResult> ListAll()
        {
            var model = await this.articlesService.GetAllAsNoTrackingFullInfoAsync<ArticleEditModel>();
            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.CreateAsync(input);

            // return this.Json(input);
            return this.RedirectToAction(nameof(this.ListAll));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleEditModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            await this.articlesService.EditAsync(input);

            return this.RedirectToAction(nameof(this.ListAll));
        }

        public async Task<IActionResult> AddSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(ArticleSuppliersModel input)
        {
            var article = await this.articlesService.GetByIdAsync(input.Id);
            await this.articlesService.AddSupplierAsync(article, input.Supplier.Id);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> ChangeMainSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleChangeMainSupplierModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMainSupplier(ArticleChangeMainSupplierModel input)
        {
            await this.articlesService.ChangeMainSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> RemoveSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSupplier(ArticleSuppliersModel input)
        {
            await this.articlesService.RemoveSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        //public async Task<IActionResult> AddConformityType(string id)
        //{
        //    var model = await this.articlesService.GetByIdAsync<AddConformityToArticleModel>();

        //    return this.View(model);
        //}

        // TODO
        public IActionResult Details(int id)
        {
            return this.View();
        }
    }
}
