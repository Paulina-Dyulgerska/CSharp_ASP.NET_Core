namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;

    public class ArticlesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            // Add articles:
            // IArticleService articleService = new ArticleService(dbContext);

            if (!dbContext.Articles.Any())
            {
                var jsonArticles = File.ReadAllText
                    ("../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ArticlesData.json");
                var articleImportDTOs = JsonSerializer.Deserialize<IEnumerable<ArticleImportDTO>>(jsonArticles);

                foreach (var articleImportDTO in articleImportDTOs)
                {
                    try
                    {
                        var articleEntity = dbContext
                                            .Articles
                                            .FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

                        if (articleEntity != null) //TODO - async-await
                        {
                            throw new ArgumentException($"There is already an article with this number.");
                        }

                        var article = new Article
                        {
                            Number = articleImportDTO.Number.Trim().ToUpper(),
                            Description = this.PascalCaseConverter(articleImportDTO.Description),
                        };

                        await dbContext.Articles.AddAsync(article);
                        await dbContext.SaveChangesAsync();

                        if (articleImportDTO.SupplierName != null && articleImportDTO.SupplierNumber != null)
                        {
                            await this.AddSupplierToArticleAsync(dbContext, article, articleImportDTO);
                        }
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

        public async Task<int> AddSupplierToArticleAsync(
            ApplicationDbContext dbContext,
            Article article,
            ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = await this.GetOrCreateSupplierAsync(dbContext, articleImportDTO);

            var articleSuppliers = dbContext.Articles
                .Where(a => a.Id == article.Id).Select(a => a.ArticleSuppliers).FirstOrDefault(); //async-await

            if (articleSuppliers.Any(x => x.SupplierId == supplierEntity.Id))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            article.ArticleSuppliers.Add(new ArticleSupplier { Supplier = supplierEntity });

            return await dbContext.SaveChangesAsync();
        }

        public async Task<Supplier> GetOrCreateSupplierAsync(
             ApplicationDbContext dbContext,
             ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = dbContext.Suppliers
                .FirstOrDefault(x => x.Number == articleImportDTO.SupplierNumber.Trim().ToUpper());

            // new supplier is created if not exist in the dbContext:
            if (supplierEntity == null)
            {
                supplierEntity = new Supplier
                {
                    Number = articleImportDTO.SupplierNumber.Trim().ToUpper(),
                    Name = this.PascalCaseConverter(articleImportDTO.SupplierName),
                    Email = articleImportDTO.SupplierEmail?.Trim(),
                    PhoneNumber = articleImportDTO.SupplierPhoneNumber?.Trim(),
                    ContactPersonFirstName = articleImportDTO.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = articleImportDTO.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonLastName),
                };

                await dbContext.Suppliers.AddAsync(supplierEntity);

                await dbContext.SaveChangesAsync();
            }

            return supplierEntity;
        }

        private string PascalCaseConverter(string stringToFix)
        {
            var st = new StringBuilder();
            st.Append(char.ToUpper(stringToFix[0]));
            for (int i = 1; i < stringToFix.Length; i++)
            {
                st.Append(char.ToLower(stringToFix[i]));
            }

            return st.ToString().Trim();
        }
    }
}
