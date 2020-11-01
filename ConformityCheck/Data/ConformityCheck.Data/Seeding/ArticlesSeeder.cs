namespace ConformityCheck.Data.Seeding
{
    using ConformityCheck.Services;
    using ConformityCheck.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ArticlesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            // Add articles:
            //IArticleService articleService = new ArticleService(dbContext);
            var jsonArticles = File.ReadAllText("ArticlesData.json");
            var articles = JsonSerializer.Deserialize<IEnumerable<ArticleImportDTO>>(jsonArticles);

            foreach (var article in articles)
            {
                try
                {
                    //serviceProvider.Create(article);
                }
                catch (Exception)
                {
                    //throw; //az ne znam kakvo shte pravq s tazi error i prodyljavam natatyk, kato prosto nqma
                    //da zapisha nishto v dbContext-a. No moqt Service throwna error i toj se hvana tuk - ot klienta na 
                    //moq Service, no kojto shte polzwa tozi cod, shte reshi kakwo da pravi s error-a. Az samo mu davam 
                    //info za towa kakwo ne e nared, towa mu prashta Service na tozi, kojto go polzwa. Towa da pravi Service
                    //error na polzwashtiqt go, e pravilnoto povedenie na Service-to!!!
                }
            }
        }

    }
}
