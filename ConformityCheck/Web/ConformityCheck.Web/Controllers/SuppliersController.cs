namespace ConformityCheck.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> ListAll()
        {
            var model = await this.suppliersService.GetAllAsNoTrackingOrderedAsync<SupplierFullInfoModel>();

            return this.View(model);
        }

        //public async Task<IActionResult> ListAll(int id = 1, int itemsPerPage = 12)
        //{
        //    if (id <= 0)
        //    {
        //        return this.NotFound(); //zarejdam StatusCodeError404.cshtml!!!
        //    }

        //    const int IntervalOfPagesToShow = 2;

        //    var model = new SuppliersListAllModel
        //    {
        //        ItemsPerPage = itemsPerPage,
        //        PageNumber = id,
        //        ItemsCount = this.suppliersService.GetCount(),
        //        IntervalOfPagesToShow = IntervalOfPagesToShow,
        //        Suppliers = await this.suppliersService
        //                        .GetAllAsNoTrackingOrderedAsPagesAsync<SupplierFullInfoModel>(id, itemsPerPage),
        //    };

        //    return this.View(model);
        //}


        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SupplierCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.suppliersService.CreateAsync(input, user.Id);

            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.suppliersService.GetByIdAsync<SupplierEditInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(SupplierEditInputModel input)
        {
            // NEVER FORGET async-await + Task<IActionResult>!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.suppliersService.EditAsync(input, user.Id);

            // TODO: return this.RedirectToAction(nameof(this.Details), "Suppliers", new { input.Id });
            return this.RedirectToAction(nameof(this.ListAll));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            //var model = await this.suppliersService.GetByIdAsync<SupplierDetailsModel>(id);
            var model = await this.suppliersService.DetailsByIdAsync(id);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.suppliersService.DeleteAsync(id);

            return this.View();
        }

        //[Authorize]
        public async Task<IActionResult> GetArticlesById(string id)
        {
            var model = await this.suppliersService.GetArticlesByIdAsync<ArticleBySupplierModel>(id);

            return this.Json(model);
        }
    }
}
