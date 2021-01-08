namespace ConformityCheck.Web.Controllers
{
    using System.Diagnostics;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    // 1. ApplicationDbContext
    // 2. Repositories
    // 3. Service
    public class HomeController : BaseController
    {
        private readonly IGetCountsService getCountsService;

        public HomeController(IGetCountsService getCountsService)
        {
            this.getCountsService = getCountsService;
        }

        public IActionResult Index()
        {
            var countsDto = this.getCountsService.GetCounts();
            var model = new IndexViewModel
            {
                Articles = countsDto.Articles,
                Conformities = countsDto.Conformities,
                ConformityTypes = countsDto.ConformityTypes,
                Products = countsDto.Products,
                RegulationLists = countsDto.RegulationLists,
                Substances = countsDto.Substances,
                Suppliers = countsDto.Suppliers,
            };

            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View($"~/Views/Shared/StatusCodeError{errorCode}.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
