namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
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

        public IActionResult Index()
        {
            var articles = this.articlesService.GetAll<ArticleExportModel>();
            var model = new ArticleListExportModel { Articles = articles };
            return this.View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateArticleInputModel
            {
                Suppliers = this.supplierService.GetAll<SupplierNumberExportModel>(),
                ConformityTypes = this.conformityTypeService.GetAll<ConformityTypeNumberExportModel>(),
                Products = this.productService.GetAll<ProductNumberExportModel>(),
                Substances = this.substanceService.GetAll<SubstanceNumberExportModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel input)
        {
            // check for error in model:
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                return this.View();
            }

            //var random = new Random();

            //// TODO - form for articles add
            //var article = new Article
            //{
            //    Number = $"Number_{random.Next()}",
            //    Description = $"Random_Generated_Article{random.Next()}",
            //    //UserId = 
            //};

            //await this.repository.AddAsync(article);
            //await this.repository.SaveChangesAsync();
            //// TODO trqbwa da e taka sled kato opravq AutoMappera:
            ////await this.articlesService.CreateAsync(new Services.Data.Models.ArticleImportDTO 
            ////{

            ////});
            ////i da iztriq repository!!!!

            //return this.RedirectToAction(nameof(this.Index));

            // TODO: redirect to artice's own page by successful creation.
            await this.articlesService.CreateAsync();

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
