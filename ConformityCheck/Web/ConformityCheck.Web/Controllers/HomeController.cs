namespace ConformityCheck.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    // 1. ApplicationDbContext
    // 2. Repositories
    // 3. Service
    public class HomeController : BaseController
    {
        private readonly IGetCountsService getCountsService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IGetCountsService getCountsService, ILogger<HomeController> logger)
        {
            this.getCountsService = getCountsService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var counts = await this.getCountsService.GetCounts();
            var model = new IndexViewModel
            {
                Articles = counts.Articles,
                Conformities = counts.Conformities,
                ConformityTypes = counts.ConformityTypes,
                Products = counts.Products,
                RegulationLists = counts.RegulationLists,
                Substances = counts.Substances,
                Suppliers = counts.Suppliers,
                Roles = counts.Roles,
                Users = counts.Users,
            };

            this.logger.LogInformation("User opens Home page");

            return this.View(model);
        }

        public IActionResult Privacy()
        {
            this.logger.LogInformation("User opens Privacy page");

            // throw new System.Exception("Hi from Home controller Privacy page");
            return this.View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View($"~/Views/Shared/StatusCodeError{errorCode}.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            this.logger
                .LogError($"Something went wrong with Request ID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}");

            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
