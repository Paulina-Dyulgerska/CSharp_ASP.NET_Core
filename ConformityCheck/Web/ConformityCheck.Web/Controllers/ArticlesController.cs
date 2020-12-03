﻿namespace ConformityCheck.Web.Controllers
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
            var model = await this.articlesService.GetAllAsNoTrackingOrderedAsync<ArticleFullInfoModel>();
            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel input)
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
            //trqbwa li da checkwam v DB za wsqko edno id, dali go imam v bazata?

            var model = await this.articlesService.GetByIdAsync<ArticleEditModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.EditAsync(input);

            return this.RedirectToAction(nameof(this.Details), "Articles", new { input.Id });
        }

        public async Task<IActionResult> AddSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.AddSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> ChangeMainSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMainSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.ChangeMainSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> RemoveSupplier(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageSuppliersModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSupplier(ArticleManageSuppliersInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.RemoveSupplierAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> AddConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.AddConformityTypeAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> RemoveConformityType(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformityTypesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConformityType(ArticleManageConformityTypesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.RemoveConformityTypesAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Id });
        }

        public async Task<IActionResult> AddConformity(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleManageConformitiesModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddConformity(ArticleManageConformitiesModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.AddConformityAsync(input);

            return this.RedirectToAction(nameof(this.Edit), "Articles", new { input.Conformity.ArticleId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.articlesService.DeleteAsync(id);

            return this.View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await this.articlesService.GetByIdAsync<ArticleEditModel>(id);

            return this.View(model);
        }

        public async Task<IActionResult> GetSuppliersById(string id)
        {
            var model = await this.articlesService.GetSuppliersByIdAsync<ArticleSupplierModel>(id);

            return this.Json(model);
        }

        public async Task<IActionResult> GetConformityTypesByIdAndSupplier(
            string articleId,
            string supplierId)
        {
            var model = await this.articlesService
                .GetConformityTypesByIdAsync(articleId, supplierId);

            return this.Json(model);
        }
    }
}
