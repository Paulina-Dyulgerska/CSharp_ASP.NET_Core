namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.EntityFrameworkCore;

    public class ConformityService : IConformityService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

        public ConformityService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.usersRepository = usersRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
        }

        public int GetCount()
        {
            return this.conformitiesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformitiesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformitiesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var conformities = await this.conformitiesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return conformities;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var entity = await this.conformitiesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            return entity;
        }

        public async Task CreateAsync(ConformityCreateModel input)
        {
            var conformityType = await this.conformityTypesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == input.ConformityTypeId);

            if (conformityType == null)
            {
                throw new ArgumentException($"There is no such conformity type.");
            }

            //DONE WITH ATTRIBUTE!!!
            //var supplierEntity = await this.suppliersRepository
            //    .All()
            //    .FirstOrDefaultAsync(x => x.Id == input.SupplierId);

            //if (supplierEntity == null)
            //{
            //    throw new ArgumentException("No such supplier");
            //}

            if (input.ValidForSingleArticle)
            {
                var articleEntity = await this.articlesRepository
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.ArticleId);

                if (articleEntity == null && input.ArticleId != null)
                {
                    throw new ArgumentException("No such article");
                }

                var articleSupplierEntity = await this.articleSuppliersRepository
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(x => x.ArticleId == input.ArticleId && x.SupplierId == input.SupplierId);

                if (articleSupplierEntity == null && input.ArticleId != null)
                {
                    throw new ArgumentException("The article does not have such supplier.");
                }

                await this.AddConformityToAnArticle(input, articleSupplierEntity);

                await this.articlesRepository.SaveChangesAsync();

                return;
            }

            var articleSuppliersEntities = await this.articleSuppliersRepository
                    .All()
                    .Where(x => x.SupplierId == input.SupplierId)
                    .ToListAsync();

            // za view componenta e towa: da se vika tq w controllera i da se prenasochva kym specialno view
            // za editvane na conformity, a ne za createvane!!!!

            foreach (var articleSupplierEntity in articleSuppliersEntities)
            {
                await this.AddConformityToAnArticle(input, articleSupplierEntity);
            }

            await this.conformitiesRepository.SaveChangesAsync();
        }

        private async Task AddConformityToAnArticle(ConformityCreateModel input, ArticleSupplier articleSupplierEntity)
        {
            var articleConformityType = await this.articleConformityTypeRepository
                                    .All()
                                    .FirstOrDefaultAsync(x => x.ConformityTypeId == input.ConformityTypeId
                                                                && x.ArticleId == articleSupplierEntity.ArticleId);

            if (articleConformityType == null)
            {
                await this.articleConformityTypeRepository.AddAsync(new ArticleConformityType
                {
                    ArticleId = articleSupplierEntity.ArticleId,
                    ConformityTypeId = input.ConformityTypeId,
                });
            }

            await this.articleConformityTypeRepository.SaveChangesAsync();

            articleConformityType = await this.articleConformityTypeRepository
                                    .All()
                                    .FirstOrDefaultAsync(x => x.ConformityTypeId == input.ConformityTypeId
                                                                && x.ArticleId == articleSupplierEntity.ArticleId);

            if (articleConformityType.ConformityId != null)
            {
                var conformityEntity = await this.conformitiesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.Id == articleConformityType.ConformityId);

                this.conformitiesRepository.Delete(conformityEntity);
            }

            articleConformityType.Conformity = new Conformity
            {
                ConformityTypeId = input.ConformityTypeId,
                IssueDate = input.IssueDate.Date,
                IsAccepted = input.IsAccepted,
                AcceptanceDate = DateTime.UtcNow.Date,
                IsValid = input.IsValid,
                ValidityDate = input.IsValid ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null,
                SupplierId = input.SupplierId,
                Comments = input.Comments,
                FileUrl = "Az ne sym go naprawila oshte",
            };

            if (input.ValidityDate != null && input.IsValid)
            {
                articleConformityType.Conformity.ValidityDate = input.ValidityDate;
            }

            await this.conformitiesRepository.AddAsync(articleConformityType.Conformity);
        }

        public async Task DeleteConformityAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
