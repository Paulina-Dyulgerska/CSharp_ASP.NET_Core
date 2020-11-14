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

        public IActionResult ListAll()
        {
            var suppliers = this.suppliersService.GetAll<SupplierFullExportModel>();
            var model = new SupplierFullExportModelList { Suppliers = suppliers };
            return this.View(model);
        }

        public IActionResult Create()
        {
            //string lastNumber = this.supplierService.GetLastSupplierNumber();

            //return this.View(lastNumber);

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
    }
}
