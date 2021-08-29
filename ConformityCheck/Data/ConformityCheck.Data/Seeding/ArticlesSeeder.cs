namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Data.Models;
    using CsvHelper;
    using Microsoft.Extensions.DependencyInjection;

    public class ArticlesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Articles.Any())
            {
                return;
            }

            // CSV import:
            // var fileCsvToCSharp = "../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ArticleSupplier - Copy1.csv";
            var fileCsvToCSharp = "../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ArticleSupplier.csv";
            var articleService = serviceProvider.GetRequiredService<IArticlesSeedService>();

            using (CsvReader reader = new CsvReader(new StreamReader(fileCsvToCSharp, Encoding.UTF8), CultureInfo.InvariantCulture))
            {
                var articleImportDTOs = reader.GetRecords<ArticleImportDTO>();
                foreach (var articleImportDTO in articleImportDTOs)
                {
                    try
                    {
                        await articleService.CreateAsync(articleImportDTO);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            // JSON import:
            // var jsonArticles = File
            //     .ReadAllText("../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ArticlesData.json");
            // var articleImportDTOs = JsonSerializer.Deserialize<IEnumerable<ArticleImportDTO>>(jsonArticles);
            // var articleService = serviceProvider.GetRequiredService<IArticlesSeedService>();
            // foreach (var articleImportDTO in articleImportDTOs)
            // {
            //     try
            //     {
            //         await articleService.CreateAsync(articleImportDTO);
            //     }
            //     catch (Exception)
            //     {
            //         // throw; //az ne znam kakvo shte pravq s tazi error i prodyljavam natatyk, kato prosto nqma
            //         // da zapisha nishto v dbContext-a. No moqt Service throwna error i toj se hvana tuk - ot klienta na
            //         // moq Service, no kojto shte polzwa tozi cod, shte reshi kakwo da pravi s error-a. Az samo mu davam
            //         // info za towa kakwo ne e nared, towa mu prashta Service na tozi, kojto go polzwa. Towa da pravi Service
            //         // error na polzwashtiqt go, e pravilnoto povedenie na Service-to!!!
            //     }
            // }
        }
    }
}
