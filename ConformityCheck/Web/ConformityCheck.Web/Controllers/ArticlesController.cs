namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private readonly IArticleService articlesService;

        private readonly IDeletableEntityRepository<Article> repository;

        public ArticlesController(IArticleService articlesService, IDeletableEntityRepository<Article> repository)
        {
            this.articlesService = articlesService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var articles = this.articlesService.GetAll<ArticleExportViewModel>();
            var model = new ArticleExportListViewModel { Articles = articles };
            return this.View(model);
        }

        public async Task<IActionResult> InsertArticle()
        {
            // TOODO - input !!! with ViewModel!
            // check for error in model:
            if (!this.ModelState.IsValid)
            {
                foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
                {
                    // DoSomething(error);
                }

                // TODO: Return Error Page
            }

            // return Ok("Success!");
            {
            }

            var random = new Random();

            // TODO - form for articles add
            var article = new Article
            {
                Number = $"Number_{random.Next()}",
                Description = $"Random_Generated_Article{random.Next()}",
                //UserId = 
            };

            await this.repository.AddAsync(article);
            await this.repository.SaveChangesAsync();
            // TODO trqbwa da e taka sled kato opravq AutoMappera:
            //await this.articlesService.CreateAsync(new Services.Data.Models.ArticleImportDTO 
            //{
            
            //});

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
