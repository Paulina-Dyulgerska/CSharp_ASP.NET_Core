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

    public class ConformityTypesService : IConformityTypesService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IRepository<ArticleConformity> articleConformitiesRepository;

        public ConformityTypesService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IRepository<ArticleConformity> articleConformitiesRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.articleConformitiesRepository = articleConformitiesRepository;
        }

        public async Task CreateAsync(ConformityTypeDTO conformityTypeImputDTO)
        {
            // if no description is provided
            if (conformityTypeImputDTO.Description == null)
            {
                throw new ArgumentNullException(nameof(conformityTypeImputDTO.Description));
            }

            // if this conformity type is already in the DB
            if (this.conformityTypesRepository.All()
                .FirstOrDefault(c => c.Description.ToUpper() == conformityTypeImputDTO.Description.ToUpper()) != null)
            {
                throw new ArgumentException($"Has this conformity type {nameof(conformityTypeImputDTO.Description)}");
            }

            ConformityType conformityType = new ConformityType
            {
                Description = conformityTypeImputDTO.Description.Trim(),
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int conformityTypeId)
        {
            // if this conformity type is not in the DB
            if (this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == conformityTypeId) == null)
            {
                throw new ArgumentException($"No such conformity type");
            }

            // if this conformity type has confirmations in the DB
            if (this.articleConformitiesRepository.All().Any(ac => ac.Conformity.ConformityTypeId == conformityTypeId))
            {
                throw new ArgumentException($"Cannot delete conformity with articles assigned to it.");
            }

            this.conformityTypesRepository
                .Delete(this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == conformityTypeId));

            return this.conformityTypesRepository.SaveChangesAsync();
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

        // TODO: delete this:
        public IEnumerable<ConformityTypeDTO> ListAllConformityTypes()
        {
            return this.conformityTypesRepository.All().Select(ct => new ConformityTypeDTO
            {
                Description = ct.Description,
            }).ToList();
        }
    }
}
