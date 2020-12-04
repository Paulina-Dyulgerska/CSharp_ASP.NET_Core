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

    public class ConformitiesService : IConformitiesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

        public ConformitiesService(
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

        public async Task CreateAsync(ConformityCreateInputModel input)
        {
            if (input.ValidForSingleArticle)
            {
                await this.AddConformityToAnArticle(input);

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
                await this.AddConformityToAnArticle(input);
            }

            await this.conformitiesRepository.SaveChangesAsync();
        }

        private async Task AddConformityToAnArticle(ConformityCreateInputModel input)
        {
            var articleConformityTypes = await this.articleConformityTypeRepository
                                    .All()
                                    .Where(x => x.ConformityTypeId == input.ConformityTypeId
                                                    && x.ArticleId == input.ArticleId)
                                    .ToListAsync();
            foreach (var articleConformityType in articleConformityTypes)
            {

            }

            var articleConformityTypeSupplier = await this.articleConformityTypeRepository
                        .All()
                        .FirstOrDefaultAsync(x => x.ConformityTypeId == input.ConformityTypeId
                                        && x.ArticleId == input.ArticleId
                                        && x.Conformity.SupplierId == input.SupplierId);

            if (articleConformityTypeSupplier == null)
            {
                articleConformityTypeSupplier = new ArticleConformityType
                {
                    ArticleId = input.ArticleId,
                    ConformityTypeId = input.ConformityTypeId,
                };

                await this.articleConformityTypeRepository.AddAsync(articleConformityTypeSupplier);
            }

            await this.articleConformityTypeRepository.SaveChangesAsync();

            // TOOD pravq proverka s Where i vzimam wsichki zapisi s takiv articleId i conftypeId i ot tqh 
            // obrabotvam po razlichnite suppliers i prezenqm dali da pravq nov zasip za veche potvyrdil supplier ili ne,
            // kakto i opredelqm da se napravi nov zapis, kogato supplier-a NE E vse oshte potvyrdil towa conformity type!!!

            if (articleConformityTypeSupplier.ConformityId != null)
            {
                var conformityEntity = await this.conformitiesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.Id == articleConformityTypeSupplier.ConformityId);

                this.conformitiesRepository.Delete(conformityEntity);
            }

            articleConformityTypeSupplier.Conformity = new Conformity
            {
                ConformityTypeId = input.ConformityTypeId,
                IssueDate = input.IssueDate.Date,
                IsAccepted = input.IsAccepted,
                AcceptanceDate = DateTime.UtcNow.Date,
                ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null,
                SupplierId = input.SupplierId,
                Comments = input.Comments,
                FileUrl = "Az ne sym go naprawila oshte",
            };

            if (input.ValidityDate != null && input.IsAccepted)
            {
                articleConformityTypeSupplier.Conformity.ValidityDate = input.ValidityDate;
            }

            await this.conformitiesRepository.AddAsync(articleConformityTypeSupplier.Conformity);
        }

        public async Task DeleteConformityAsync(string id)
        {
            throw new NotImplementedException();
        }

        //Task AddConformityAsync(ArticleManageConformitiesModel input);

    }
}
