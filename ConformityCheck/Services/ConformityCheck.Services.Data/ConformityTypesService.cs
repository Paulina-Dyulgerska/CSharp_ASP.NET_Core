namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;

    public class ConformityTypesService : IConformityTypesService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;

        public ConformityTypesService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
            this.conformitiesRepository = conformitiesRepository;
        }

        public int GetCount()
        {
            return this.conformityTypesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformityTypesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformityTypesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var articles = await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Description)
                .ThenByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.conformityTypesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ConformityTypeCreateInputModel input)
        {
            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            var conformityType = new ConformityType
            {
                Description = input.Description.ToUpper(),
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ConformityTypeEditInputModel input)
        {
            // if this conformity type is not in the DB
            var entity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == input.Id);

            entity.Description = input.Description.ToUpper();

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var conformityTypeEntity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);

            // if this conformity type has confirmations in the DB
            // TODO - catch the error in Controller
            if (this.conformitiesRepository.All().Any(ac => ac.ConformityTypeId == id))
            {
                throw new ArgumentException($"Cannot delete conformity with articles assigned to it.");
            }

            this.conformityTypesRepository.Delete(conformityTypeEntity);

            return await this.conformityTypesRepository.SaveChangesAsync();
        }
    }
}
