namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Messaging;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class RequestsController : BaseController
    {
        private const string ArticlesCallerViewName = GlobalConstants.Articles;
        private const string SuppliersCallerViewName = GlobalConstants.Suppliers;
        private const string ConformitiesCallerViewName = GlobalConstants.Conformities;
        private readonly string conformityFilesDirectory;
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IProductsService productsService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly ISubstancesService substancesService;
        private readonly IConformitiesService conformitiesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender emailSender;
        private readonly ILogger<RequestsController> logger;

        public RequestsController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            IConformitiesService conformitiesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            ILogger<RequestsController> logger)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.productsService = productsService;
            this.conformityTypesService = conformityTypesService;
            this.substancesService = substancesService;
            this.conformitiesService = conformitiesService;
            this.userManager = userManager;
            this.environment = environment;
            this.emailSender = emailSender;
            this.logger = logger;
            this.conformityFilesDirectory = $"{this.environment.WebRootPath}/files";
        }

        public async Task<IActionResult> SendRequest(ConformityGetInputModel input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                var article = await this.articlesService.GetByIdAsync<ArticleBaseExportModel>(input.ArticleId);
                var supplier = await this.suppliersService.GetByIdAsync<SupplierBaseExportModel>(input.SupplierId);
                var conformityType = await this.conformityTypesService.GetByIdAsync<ConformityTypeBaseExportModel>(input.ConformityTypeId);

                // send email to supplier
                await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    supplier.Email,
                    supplier.Name,
                    $"{conformityType.Description} conformity request for article {article.Number} - {article.Description}",
                    $"Dear {supplier.Name},\r\nWe would like to kindly request a {conformityType.Description} confirmation for article {article.Number} - {article.Description}.\r\n\r\nKind Regards,\r\nConformity Check Team,\r\n{user.FirstName} {user.LastName}");

                await this.conformitiesService.AddRequestDateAsync(input, user.Id);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.RequestSentSuccessfullyMessage;
                this.logger.LogInformation($"Conformity request for article {input.ArticleId} - {input.ConformityTypeId} - was send to {input.SupplierId} by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Conformity request sending failed: {ex}");

                // if error accure here, the Home/Error page will be displayed
            }

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }
    }
}
