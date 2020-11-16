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
        private readonly ApplicationDbContext db;

        public ArticlesController(
            IArticlesService articlesService,
            ISuppliersService supplierService,
            IProductsService productService,
            IConformityTypesService conformityTypeService,
            ISubstancesService substanceService,
            ApplicationDbContext db)
        {
            this.articlesService = articlesService;
            this.supplierService = supplierService;
            this.productService = productService;
            this.conformityTypeService = conformityTypeService;
            this.substanceService = substanceService;
            this.db = db;
        }

        public IActionResult ListAll()
        {
            //var articles = this.articlesService.GetAll<ArticleExportModel>();
            //var model = new ArticleExportModelList { Articles = articles };

            var articles = this.articlesService.GetAllAsNoTrackingFullInfo();
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

        public IActionResult Details(int id)
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

        public IActionResult Edit(string id)
        {
            var model = this.articlesService.GetEditArticle(id);
            model.Create = new CreateArticleInputModel
            {
                SuppliersAvailable = this.supplierService.GetAllAsNoTracking<SupplierExportModel>(),
                ConformityTypesAvailable = this.conformityTypeService.GetAllAsNoTracking<ConformityTypeNumberModel>(),
                ProductsAvailable = this.productService.GetAllAsNoTracking<ProductNumberExportModel>(),
                SubstancesAvailable = this.substanceService.GetAllAsNoTracking<SubstanceNumberExportModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExportModel articleInputModel)
        {
            //this.db.ArticleSuppliers.Add(new ArticleSupplier
            //{
            //    ArticleId = articleInputModel.Export.Id,
            //    SupplierId = articleInputModel.Create.Supplier.Id,
            //    IsMainSupplier = true,
            //});

            //var rt = this.db.SaveChanges();

            // NEVer FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            await this.articlesService.PostEditArticleAsync(articleInputModel);

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
