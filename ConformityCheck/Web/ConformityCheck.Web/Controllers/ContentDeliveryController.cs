namespace ConformityCheck.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Administration.Users;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("/api")]
    public class ContentDeliveryController : ControllerBase
    {
        private readonly IArticlesService articlesService;
        private readonly ISuppliersService suppliersService;
        private readonly IConformityTypesService conformityTypesService;
        private readonly IConformitiesService conformitiesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ContentDeliveryController(
            IArticlesService articlesService,
            ISuppliersService suppliersService,
            IConformityTypesService conformityTypesService,
            IConformitiesService conformitiesService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.articlesService = articlesService;
            this.suppliersService = suppliersService;
            this.conformityTypesService = conformityTypesService;
            this.conformitiesService = conformitiesService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await this.articlesService.GetAllAsNoTrackingAsync<ArticleExportModel>();

            return this.Ok(model);
        }

        [HttpGet(nameof(GetArticleSuppliers) + "/{id}")]
        public async Task<IActionResult> GetArticleSuppliers(string id)
        {
            var model = await this.articlesService.GetSuppliersByIdAsync<SupplierExportModel>(id);

            return this.Ok(model);
        }

        [HttpGet(nameof(GetArticleConformityTypesBySupplier))]
        public async Task<IActionResult> GetArticleConformityTypesBySupplier(
            string articleId,
            string supplierId)
        {
            var model = await this.articlesService
                .GetConformityTypesByIdAndSupplierAsync(articleId, supplierId);

            return this.Ok(model);
        }

        [HttpGet(nameof(GetSupplierArticles) + "/{id}")]
        public async Task<IActionResult> GetSupplierArticles(string id)
        {
            try
            {
                var model = await this.suppliersService.GetArticlesByIdAsync<ArticleExportModel>(id);
                return this.Ok(model);
            }

            // TODO : ILogger!
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        [HttpGet(nameof(GetArticlesByNumberOrDescription))]
        public async Task<IActionResult> GetArticlesByNumberOrDescription(string input)
        {
            var model = await this.articlesService.GetAllBySearchInputAsync<ArticleExportModel>(input);

            return this.Ok(model);
        }

        [HttpGet(nameof(GetSuppliersByNumberOrName))]
        public async Task<IActionResult> GetSuppliersByNumberOrName(string input)
        {
            var model = await this.suppliersService.GetAllBySearchInputAsync<SupplierExportModel>(input);

            return this.Ok(model);
        }

        [HttpGet(nameof(GetConformityTypeByIdOrDescription))]
        public async Task<IActionResult> GetConformityTypeByIdOrDescription(string input)
        {
            var model = await this.conformityTypesService.GetAllBySearchInputAsync<ConformityTypeExportModel>(input);

            return this.Ok(model);
        }

        [HttpGet(nameof(GetConformitiesByArticleOrSupplierOrConformityType))]
        public async Task<IActionResult> GetConformitiesByArticleOrSupplierOrConformityType(string input)
        {
            var model = await this.conformitiesService.GetAllBySearchInputAsync<ConformityExportModel>(input);

            return this.Ok(model);
        }

        [Authorize]
        [HttpGet(nameof(ShowModalDocument))]
        public IActionResult ShowModalDocument(string conformityFileUrl)
        {
            string filePath = "~" + conformityFileUrl;
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = conformityFileUrl.Split('/').LastOrDefault(),
                Inline = true,
            };

            this.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return this.File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        [HttpGet(nameof(GetAllUsers))]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await this.userManager.Users
                .Select(x => new UserExportModel
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

            return this.Ok(users);
        }

        //[ApiController]
        //[Route("api/[controller]")]
        //public class VotesController : BaseController
        //{
        //    private readonly IVotesService votesService;

        //    public VotesController(IVotesService votesService)
        //    {
        //        this.votesService = votesService;
        //    }

        //    [HttpPost]
        //    [Authorize]
        //    public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteInputModel input)
        //    {
        //        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        await this.votesService.SetVoteAsync(input.RecipeId, userId, input.Value);
        //        var averageVotes = this.votesService.GetAverageVotes(input.RecipeId);
        //        return new PostVoteResponseModel { AverageVote = averageVotes };
        //    }
        //}
        //<form method = "post" id="antiForgeryForm"></form>
        //        @section Scripts
        //        {
        //    <script>
        //        $("li[data-vote]").each(function (el) {
        //            $(this).click(function() {
        //                var value = $(this).attr("data-vote");
        //                var recipeId = @Model.Id;
        //                var antiForgeryToken = $('#antiForgeryForm input[name=__RequestVerificationToken]').val();
        //                var data = { recipeId: recipeId, value: value };
        //                $.ajax({
        //            type: "POST",
        //                    url: "/api/Votes",
        //                    data: JSON.stringify(data),
        //                    headers:
        //                {
        //                    'X-CSRF-TOKEN': antiForgeryToken
        //                    },
        //                    success: function(data) {
        //                        $('#averageVoteValue').html(data.averageVote.toFixed(1));
        //                },
        //                    contentType: 'application/json',
        //                });
        //        })
        //        });
        //    </script>
        //}
    }
}
