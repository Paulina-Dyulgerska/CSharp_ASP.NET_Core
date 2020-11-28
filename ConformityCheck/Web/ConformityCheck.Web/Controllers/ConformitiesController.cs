namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.AspNetCore.Mvc;

    public class ConformitiesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;
        private readonly IConformityService conformityService;

        public ConformitiesController(
            IArticlesService articlesService,
            ISuppliersService supplierService,
            IProductsService productService,
            IConformityTypesService conformityTypeService,
            ISubstancesService substanceService,
            IConformityService conformityService)
        {
            this.articlesService = articlesService;
            this.supplierService = supplierService;
            this.productService = productService;
            this.conformityTypeService = conformityTypeService;
            this.substanceService = substanceService;
            this.conformityService = conformityService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConformityCreateModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.ValidForSingleArticle = false;

                return this.View(input);
            }

            await this.conformityService.CreateAsync(input);

            // return this.Json(input);
            return this.RedirectToAction("ListAll", "Articles");
        }
    }
}
