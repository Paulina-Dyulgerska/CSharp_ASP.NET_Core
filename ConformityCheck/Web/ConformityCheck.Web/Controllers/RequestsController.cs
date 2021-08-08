namespace ConformityCheck.Web.Controllers
{
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

    [Authorize]
    public class RequestsController : BaseController
    {
        private const string ArticlesCallerViewName = "Articles";
        private const string SuppliersCallerViewName = "Suppliers";
        private const string ConformitiesCallerViewName = "Conformities";
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

        public RequestsController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IProductsService productsService,
            IConformityTypesService conformityTypesService,
            ISubstancesService substancesService,
            IConformitiesService conformitiesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailSender emailSender)
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
            this.conformityFilesDirectory = $"{this.environment.WebRootPath}/files";
        }

        public async Task<IActionResult> SendRequest(ConformityGetInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var article = await this.articlesService.GetByIdAsync<ArticleBaseModel>(input.ArticleId);
            var supplier = await this.suppliersService.GetByIdAsync<SupplierBaseModel>(input.SupplierId);
            var conformityType = await this.conformityTypesService.GetByIdAsync<ConformityTypeBaseModel>(input.ConformityTypeId);

            // send email to site admin
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                supplier.Email,
                supplier.Name,
                $"{conformityType.Description} conformity request for article {article.Number} - {article.Description}",
                $"Dear {supplier.Name},\r\nWe would like to kindly request a {conformityType.Description} confirmation for article {article.Number} - {article.Description}.\r\n\r\nKind Regards,\r\nConformity Check Team,\r\n{user.FirstName} {user.LastName}");

            await this.articlesService.AddRequestDateAsync(input);

            this.TempData[GlobalConstants.TempDataMessagePropertyName] =
                GlobalConstants.RequestSentSuccessfullyMessage;

            if (input.CallerViewName == SuppliersCallerViewName)
            {
                return this.RedirectToAction(nameof(SuppliersController.Details), SuppliersCallerViewName, new { id = input.SupplierId });
            }

            return this.RedirectToAction(nameof(ArticlesController.Details), ArticlesCallerViewName, new { id = input.ArticleId });
        }
    }
}
