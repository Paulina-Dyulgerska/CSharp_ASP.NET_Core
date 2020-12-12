namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticlesSeedService : IArticlesSeedService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;

        public ArticlesSeedService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
        }

        public async Task CreateAsync(ArticleImportDTO articleImportDTO)
        {
            var articleEntity = this.articlesRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

            // TODO - async-await
            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var article = new Article
            {
                Number = articleImportDTO.Number.Trim().ToUpper(),
                Description = this.PascalCaseConverter(articleImportDTO.Description),

                // TODO: UserId = userEntity.Id -> AdminUser
            };

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
                                    .AllAsNoTracking()
                                    .FirstOrDefault(x => x.Description.ToUpper() == "ROHS").Id,
            });

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
                        .AllAsNoTracking()
                        .FirstOrDefault(x => x.Description.ToUpper() == "DS_Substances").Id,
            });

            article.ArticleConformityTypes.Add(new ArticleConformityType
            {
                ConformityTypeId = this.conformityTypesRepository
            .AllAsNoTracking()
            .FirstOrDefault(x => x.Description.ToUpper() == "SVHC").Id,
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
                    Email = articleImportDTO.SupplierEmail?.Trim(),
                    PhoneNumber = articleImportDTO.SupplierPhoneNumber?.Trim(),
                    ContactPersonFirstName = articleImportDTO.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = articleImportDTO.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonLastName),
                };

                await this.suppliersRepository.AddAsync(supplierEntity);

                await this.suppliersRepository.SaveChangesAsync();
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
