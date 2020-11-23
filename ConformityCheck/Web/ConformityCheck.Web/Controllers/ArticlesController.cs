namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;
    using ConformityCheck.Data.Models;
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
            var model = await this.articlesService.GetAllAsNoTrackingFullInfoAsync<ArticleFullInfoModel>();
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
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(ArticleManageSuppliersModel input)
        {
            var article = await this.articlesService.GetByIdAsync(input.Id);
            await this.articlesService.AddSupplierAsync(article, input.SupplierId);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> ChangeMainSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMainSupplier(ArticleManageSuppliersModel input)
        {
            await this.articlesService.ChangeMainSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> RemoveSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSupplier(ArticleManageSuppliersModel input)
        {
            await this.articlesService.RemoveSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> AddConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddConformityType(ArticleManageConformityTypesModel input)
        {
            await this.articlesService.AddConformityTypesAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> RemoveConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConformityType(ArticleManageConformityTypesModel input)
        {
            await this.articlesService.RemoveConformityTypesAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.articlesService.DeleteAsync(id);

            return this.View();
        }

        // TODO
        public IActionResult Details(int id)
        {
            return this.View();
        }
    }
}
