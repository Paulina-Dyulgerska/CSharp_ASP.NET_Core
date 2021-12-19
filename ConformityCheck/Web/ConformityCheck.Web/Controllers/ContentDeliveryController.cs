namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Administration.Users;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.ContentDelivery;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("/api")]
    public class ContentDeliveryController : ControllerBase
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly IConformitiesService conformitiesService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<ContentDeliveryController> logger;
        private readonly BlobServiceClient blobServiceClient;

        public ContentDeliveryController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IConformityTypesService conformityTypesService,
            IConformitiesService conformitiesService,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<ContentDeliveryController> logger,
            BlobServiceClient blobServiceClient)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.conformityTypesService = conformityTypesService;
            this.conformitiesService = conformitiesService;
            this.usersService = usersService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.blobServiceClient = blobServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {
                var model = await this.articlesService.GetAllAsNoTrackingAsync<ArticleExportModel>();

                this.logger.LogInformation($"Api {nameof(this.GetAllArticles)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetArticleSuppliers) + "/{id}")]
        public async Task<IActionResult> GetArticleSuppliers([FromRoute] ArticleIdInputModel input)
        {
            // TODO - delete, not needed, API filter is doing this:
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            try
            {
                var model = await this.articlesService.GetSuppliersByIdAsync<SupplierExportModel>(input.Id);

                this.logger.LogInformation($"Api {nameof(this.GetArticleSuppliers)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetArticleConformityTypesBySupplier))]
        public async Task<IActionResult> GetArticleConformityTypesBySupplier([FromQuery] ArticleIdSupplierIdInputModel input)
        {
            // TODO - delete, not needed, API filter is doing this:
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            try
            {
                var model = await this.articlesService.GetConformityTypesByIdAndSupplierAsync(input.ArticleId, input.SupplierId);

                this.logger.LogInformation($"Api {nameof(this.GetArticleConformityTypesBySupplier)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetSupplierArticles) + "/{id}")]
        public async Task<IActionResult> GetSupplierArticles([FromRoute] SupplierIdInputModel input)
        {
            // TODO - delete, not needed, API filter is doing this:
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            try
            {
                var model = await this.suppliersService.GetArticlesByIdAsync<ArticleExportModel>(input.Id);

                this.logger.LogInformation($"Api {nameof(this.GetSupplierArticles)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetArticlesByNumberOrDescription))]
        public async Task<IActionResult> GetArticlesByNumberOrDescription(string input)
        {
            try
            {
                var model = await this.articlesService.GetAllBySearchInputAsync<ArticleExportModel>(input);

                this.logger.LogInformation($"Api {nameof(this.GetArticlesByNumberOrDescription)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetSuppliersByNumberOrName))]
        public async Task<IActionResult> GetSuppliersByNumberOrName(string input)
        {
            try
            {
                var model = await this.suppliersService.GetAllBySearchInputAsync<SupplierExportModel>(input);

                this.logger.LogInformation($"Api {nameof(this.GetSuppliersByNumberOrName)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetConformityTypeByIdOrDescription))]
        public async Task<IActionResult> GetConformityTypeByIdOrDescription(string input)
        {
            try
            {
                var model = await this.conformityTypesService.GetAllBySearchInputAsync<ConformityTypeExportModel>(input);

                this.logger.LogInformation($"Api {nameof(this.GetConformityTypeByIdOrDescription)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [HttpGet(nameof(GetConformitiesByArticleOrSupplierOrConformityType))]
        public async Task<IActionResult> GetConformitiesByArticleOrSupplierOrConformityType(string input)
        {
            try
            {
                var model = await this.conformitiesService.GetAllBySearchInputAsync<ConformityExportModel>(input);

                this.logger.LogInformation($"Api {nameof(this.GetConformitiesByArticleOrSupplierOrConformityType)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet(nameof(GetUserByUsernameOrEmailOrRole))]
        public IActionResult GetUserByUsernameOrEmailOrRole(string input)
        {
            try
            {
                var model = this.usersService.GetAllBySearchInput(input);

                this.logger.LogInformation($"Api {nameof(this.GetUserByUsernameOrEmailOrRole)} success.");

                return this.Ok(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [Authorize]
        [HttpGet(nameof(ShowModalDocument))]
        public async Task<IActionResult> ShowModalDocument(string conformityFileUrl)
        {
            try
            {
                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = conformityFileUrl.Split('/').LastOrDefault(),
                    Inline = true,
                };

                this.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                var fileSavedInRemoteStorageContainer = conformityFileUrl.StartsWith("http");

                if (!fileSavedInRemoteStorageContainer)
                {
                    var fileData = this.conformitiesService.GetConformityFileFromLocalStorage(conformityFileUrl);

                    this.logger.LogInformation($"API {nameof(this.ShowModalDocument)} from local storage success.");

                    return this.File(fileData.FilePath, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
                else
                {
                    var fileData = await this.conformitiesService.GetConformityFileFromBlobStorage(conformityFileUrl);

                    this.logger.LogInformation($"API {nameof(this.ShowModalDocument)} from remote storage success.");

                    // return file as shown in new tab from Blob:
                    return this.File(fileData.FileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf);

                    // return file as downloaded from Blob:
                    // return this.File(fileData.FileBytes, fileData.FileContentType);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet(nameof(GetAllUsers))]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // with AutoMapper - not nessessary since only 1 time is used. If more than 1 times
                // needed, put the AutoMapperConfig in Startup.Configure() method and delete the Select():
                // AutoMapperConfig.RegisterMappings(typeof(ApplicationUser).GetTypeInfo().Assembly);
                // var users = await this.userManager.Users
                //                                   .To<UserExportModel>()
                //                                   .ToListAsync();
                var users = await this.userManager.Users
                                                   .Select(x => new UserListAllViewModel
                                                   {
                                                       Id = x.Id,
                                                       UserName = x.UserName,
                                                       Email = x.Email,
                                                       Roles = x.Roles.Select(r => r.RoleId).ToArray(),
                                                   })
                                                   .ToListAsync();

                foreach (var user in users)
                {
                    if (user.Roles.Length > 0)
                    {
                        for (int i = 0; i < user.Roles.Length; i++)
                        {
                            var role = await this.roleManager.FindByIdAsync(user.Roles[i]);
                            user.Roles[i] = role.Name;
                        }
                    }
                }

                this.logger.LogInformation($"Api {nameof(this.GetAllUsers)} success.");

                return this.Ok(users);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        // [ApiController]
        // [Route("api/[controller]")]
        // public class VotesController : BaseController
        // {
        //     private readonly IVotesService votesService;
        //     public VotesController(IVotesService votesService)
        //     {
        //         this.votesService = votesService;
        //     }
        //     [HttpPost]
        //     [Authorize]
        //     public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteInputModel input)
        //     {
        //       var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //         await this.votesService.SetVoteAsync(input.RecipeId, userId, input.Value);
        //         var averageVotes = this.votesService.GetAverageVotes(input.RecipeId);
        //         return new PostVoteResponseModel { AverageVote = averageVotes };
        //     }
        // }
        // <form method = "post" id="antiForgeryForm"></form>
        //         @section Scripts
        //         {
        //     <script>
        //         $("li[data-vote]").each(function (el) {
        //             $(this).click(function() {
        //                 var value = $(this).attr("data-vote");
        //                 var recipeId = @Model.Id;
        //                 var antiForgeryToken = $('#antiForgeryForm input[name=__RequestVerificationToken]').val();
        //                 var data = { recipeId: recipeId, value: value };
        //                 $.ajax({
        //             type: "POST",
        //                     url: "/api/Votes",
        //                     data: JSON.stringify(data),
        //                     headers:
        //                 {
        //                     'X-CSRF-TOKEN': antiForgeryToken
        //                     },
        //                     success: function(data) {
        //                         $('#averageVoteValue').html(data.averageVote.toFixed(1));
        //                 },
        //                     contentType: 'application/json',
        //                 });
        //         })
        //         });
        //     </script>
        // }
    }
}
