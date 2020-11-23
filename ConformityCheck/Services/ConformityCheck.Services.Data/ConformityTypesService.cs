namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.EntityFrameworkCore;

    public class ConformityTypesService : IConformityTypesService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

        public ConformityTypesService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
        }

        public int GetCount()
        {
            return this.conformityTypesRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.conformityTypesRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking<T>()
        {
            return this.conformityTypesRepository.AllAsNoTracking().To<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformityTypesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingFullInfoAsync<T>()
        {
            var articles = await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Description)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task CreateAsync(ConformityTypeModel input)
        {
            // if no description is provided
            if (input.Description == null)
            {
                throw new ArgumentNullException(nameof(input.Description));
            }

            // if this conformity type is already in the DB
            if (this.conformityTypesRepository.All()
                .FirstOrDefault(c => c.Description.ToUpper() == input.Description.ToUpper()) != null)
            {
                throw new ArgumentException($"Has this conformity type {nameof(input.Description)}");
            }

            ConformityType conformityType = new ConformityType
            {
                Description = input.Description.Trim(),
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            // if this conformity type is not in the DB
            if (this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == id) == null)
            {
                throw new ArgumentException($"No such conformity type");
            }

            // if this conformity type has confirmations in the DB
            if (this.articleConformityTypeRepository.All().Any(ac => ac.Conformity.ConformityTypeId == id))
            {
                throw new ArgumentException($"Cannot delete conformity with articles assigned to it.");
            }

            this.conformityTypesRepository
                .Delete(this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == id));

            return this.conformityTypesRepository.SaveChangesAsync();
        }
    }
}
