namespace ConformityCheck.Web.Controllers
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.AspNetCore.Mvc;

    public class ConformityTypesController : Controller
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService supplierService;
        private readonly IProductsService productService;
        private readonly IConformityTypesService conformityTypeService;
        private readonly ISubstancesService substanceService;

        public ConformityTypesController(
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
            var model = await this.conformityTypeService.GetAllAsNoTrackingOrderedAsync<ConformityTypeExportModel>();

            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConformityTypeCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.conformityTypeService.CreateAsync(input);

            return this.RedirectToAction(nameof(this.ListAll));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.conformityTypeService.GetByIdAsync<ConformityTypeEditInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConformityTypeEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.conformityTypeService.EditAsync(input);

            // TODO: return this.RedirectToAction(nameof(this.Details), "ConformityTypes", new { input.Id });
            return this.RedirectToAction(nameof(this.ListAll));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.conformityTypeService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.ListAll));
        }
    }
}
