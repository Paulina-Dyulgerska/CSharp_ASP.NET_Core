﻿namespace ConformityCheck.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using ConformityCheck.Web.ViewModels.Suppliers.ViewComponents;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Mvc;
    using ConformityCheck.Web.ViewModels.Articles;

    public class SuppliersController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;

        public SuppliersController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
        }

        public async Task<IActionResult> ListAll()
        {
            var model = await this.suppliersService.GetAllAsync<SupplierFullInfoModel>();

            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                return this.View(input);
            }

            await this.suppliersService.CreateAsync(input);

            return this.RedirectToAction(nameof(this.ListAll));

            //return this.Json(input);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.suppliersService.GetByIdAsync<SupplierEditModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.suppliersService.EditAsync(input);

            // TODO: return this.RedirectToAction(nameof(this.Details), "Suppliers", new { input.Id });
            return this.RedirectToAction(nameof(this.ListAll));
        }

        public async Task<IActionResult> GetArticlesById(string id)
        {
            var model = await this.suppliersService.GetArticlesByIdAsync<ArticleBaseModel>(id);

            return this.Json(model);
        }
    }
}
