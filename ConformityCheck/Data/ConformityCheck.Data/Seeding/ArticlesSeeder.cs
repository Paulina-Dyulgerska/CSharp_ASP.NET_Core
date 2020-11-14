﻿namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class ArticlesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Articles.Any())
            {
                return;
            }

            var jsonArticles = File
                .ReadAllText("../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ArticlesData.json");
            var articleImportDTOs = JsonSerializer.Deserialize<IEnumerable<ArticleImportDTO>>(jsonArticles);
            var articleService = serviceProvider.GetRequiredService<IArticlesService>();

            foreach (var articleImportDTO in articleImportDTOs)
            {
                try
                {
                    await articleService.CreateAsync(articleImportDTO);
                }
                catch (Exception)
                {
                    // throw; //az ne znam kakvo shte pravq s tazi error i prodyljavam natatyk, kato prosto nqma
                    // da zapisha nishto v dbContext-a. No moqt Service throwna error i toj se hvana tuk - ot klienta na
                    // moq Service, no kojto shte polzwa tozi cod, shte reshi kakwo da pravi s error-a. Az samo mu davam
                    // info za towa kakwo ne e nared, towa mu prashta Service na tozi, kojto go polzwa. Towa da pravi Service
                    // error na polzwashtiqt go, e pravilnoto povedenie na Service-to!!!
                }
            }
        }
    }
}
