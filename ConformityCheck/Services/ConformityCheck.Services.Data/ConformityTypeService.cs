namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;

    public class ConformityTypeService : IConformityTypeService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IRepository<ArticleConformity> articleConformitiesRepository;

        public ConformityTypeService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IRepository<ArticleConformity> articleConformitiesRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.articleConformitiesRepository = articleConformitiesRepository;
        }

        public void Create(ConformityTypeDTO conformityTypeImputDTO)
        {
            //if no description is provided
            if (conformityTypeImputDTO.Description == null)
            {
                throw new ArgumentNullException(nameof(conformityTypeImputDTO.Description));
            }

            //if this conformity type is already in the DB
            if (this.conformityTypesRepository.All()
                .FirstOrDefault(c => c.Description.ToUpper() == conformityTypeImputDTO.Description.ToUpper()) != null)
            {
                throw new ArgumentException($"Has this conformity type {nameof(conformityTypeImputDTO.Description)}");
            }

            ConformityType conformityType = new ConformityType
            {
                Description = conformityTypeImputDTO.Description.Trim(),
            };

            this.conformityTypesRepository.AddAsync(conformityType);

            this.conformityTypesRepository.SaveChangesAsync();
        }

        public Task<int> Delete(int conformityTypeId)
        {
            //if this conformity type is not in the DB
            if (this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == conformityTypeId) == null)
            {
                throw new ArgumentException($"No such conformity type");
            }

            //if this conformity type has confirmations in the DB
            if (this.articleConformitiesRepository.All().Any(ac => ac.ConformityId == conformityTypeId))
            {
                throw new ArgumentException($"Cannot delete conformity with articles assigned to it.");
            }

            this.conformityTypesRepository
                .Delete(this.conformityTypesRepository.All().FirstOrDefault(c => c.Id == conformityTypeId));

            return this.conformityTypesRepository.SaveChangesAsync();
        }

        public IEnumerable<ConformityTypeDTO> ListAllConformityTypes()
        {
            return this.conformityTypesRepository.All().Select(ct => new ConformityTypeDTO
            {
                Description = ct.Description,
            }).ToList();
        }
    }
}
