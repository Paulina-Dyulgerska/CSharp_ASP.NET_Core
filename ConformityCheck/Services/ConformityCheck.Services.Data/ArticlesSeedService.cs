namespace ConformityCheck.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class ArticlesSeedService : IArticlesSeedService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IServiceProvider serviceProvider;

        public ArticlesSeedService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IServiceProvider serviceProvider)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.serviceProvider = serviceProvider;
        }

        public async Task CreateAsync(ArticleImportDTO articleImportDTO)
        {
            var userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);

            var articleEntity = this.articlesRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var random = new Random();
            var numb = random.Next(100000000, 999999999);
            var hasSuchNumber = this.articlesRepository.AllAsNoTrackingWithDeleted()
                .Any(x => x.Number == numb.ToString());
            while (hasSuchNumber)
            {
                numb = random.Next(100000000, 999999999);
                hasSuchNumber = this.articlesRepository.AllAsNoTrackingWithDeleted()
                .Any(x => x.Number == numb.ToString());
            }

            var article = new Article
            {
                // Number = articleImportDTO.Number.Trim().ToUpper(),
                Number = numb.ToString(),
                Description = PascalCaseConverter.Convert(articleImportDTO.Description),
                User = adminUsers.FirstOrDefault(),
            };

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Description.ToUpper() == "RoHS".ToUpper()).Id,
            });

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Description.ToUpper() == "DS_Substances".ToUpper()).Id,
            });

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Description.ToUpper() == "SVHC".ToUpper()).Id,
            });

            await this.articlesRepository.AddAsync(article);

            await this.articlesRepository.SaveChangesAsync();

            if (articleImportDTO.SupplierName != null && articleImportDTO.SupplierNumber != null)
            {
                await this.AddSupplierToArticleAsync(article, articleImportDTO);
            }
        }

        private async Task<int> AddSupplierToArticleAsync(Article article, ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = await this.GetOrCreateSupplierAsync(articleImportDTO);

            var articleSuppliers = this.articlesRepository.All()
                .Where(a => a.Id == article.Id).Select(a => a.ArticleSuppliers).FirstOrDefault();

            if (articleSuppliers.Any(x => x.SupplierId == supplierEntity.Id))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            article.ArticleSuppliers.Add(new ArticleSupplier { Supplier = supplierEntity, IsMainSupplier = true });

            return await this.articlesRepository.SaveChangesAsync();
        }

        private async Task<Supplier> GetOrCreateSupplierAsync(ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = this.suppliersRepository.All()
                .FirstOrDefault(x => x.Number == articleImportDTO.SupplierNumber.Trim().ToUpper());

            // new supplier is created if not exist in the dbContext:
            if (supplierEntity == null)
            {
                supplierEntity = new Supplier
                {
                    Number = articleImportDTO.SupplierNumber.Trim().ToUpper(),
                    Name = articleImportDTO.SupplierName.Trim().ToUpper(),
                    Email = "paylina_st@yahoo.com",
                    PhoneNumber = articleImportDTO.SupplierPhoneNumber?.Trim(),
                    ContactPersonFirstName = articleImportDTO.ContactPersonFirstName == null ? null :
                            PascalCaseConverter.Convert(articleImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = articleImportDTO.ContactPersonLastName == null ? null :
                            PascalCaseConverter.Convert(articleImportDTO.ContactPersonLastName),
                };

                await this.suppliersRepository.AddAsync(supplierEntity);

                await this.suppliersRepository.SaveChangesAsync();
            }

            return supplierEntity;
        }
    }
}
