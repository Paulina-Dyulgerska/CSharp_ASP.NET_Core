namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;

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

            this.logger.LogInformation("User opens Home page");

            return this.View(model);
        }

        public IActionResult Privacy()
        {
            this.logger.LogInformation("User opens Privacy page");
            throw new ArgumentException("Subject and message should be provided.");

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
