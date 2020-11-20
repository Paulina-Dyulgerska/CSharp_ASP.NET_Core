namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Data;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Products;
    using ConformityCheck.Web.ViewModels.Substances;
    using ConformityCheck.Web.ViewModels.Suppliers;
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
            //var articles = this.articlesService.GetAll<ArticleExportModel>();
            //var model = new ArticleExportModelList { Articles = articles };

            var articles = await this.articlesService.GetAllAsNoTrackingFullInfoAsync<EditExportModel>();
            var model = new EditExportModelList { Articles = articles };
            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                //foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                //{
                //    return this.View(input);
                //}

                return this.View(input);
            }

            await this.articlesService.CreateAsync(input);

            return this.RedirectToAction(nameof(this.ListAll));

            //return this.Json(input);
        }

        public IActionResult Details(int id)
        {

            return this.View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.articlesService.GetEditAsync(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExportModel articleInputModel)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            await this.articlesService.PostEditAsync(articleInputModel);

            return this.RedirectToAction(nameof(this.ListAll));
        }

        public IActionResult AddSupplier(string id)
        {
            return this.RedirectToAction(nameof(this.Edit));
        }

        public IActionResult DeleteSupplier(string id)
        {
            return this.RedirectToAction(nameof(this.Edit));
        }
    }
}
