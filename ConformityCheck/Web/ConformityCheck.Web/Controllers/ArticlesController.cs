namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IActionResult ListAll()
        {
            var articles = this.articlesService.GetAll<ArticleExportModel>();
            var model = new ArticleExportModelList { Articles = articles };
            return this.View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateArticleInputModel
            {
                SuppliersAvailable = this.supplierService.GetAllAsNoTracking<SupplierExportModel>(),
                ConformityTypesAvailable = this.conformityTypeService.GetAllAsNoTracking<ConformityTypeNumberModel>(),
                ProductsAvailable = this.productService.GetAllAsNoTracking<ProductNumberExportModel>(),
                SubstancesAvailable = this.substanceService.GetAllAsNoTracking<SubstanceNumberExportModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                return this.View(input);
            }

            await this.articlesService.CreateAsync(input);

            return this.RedirectToAction(nameof(this.ListAll));

            //return this.Json(input);
        }
    }
}
