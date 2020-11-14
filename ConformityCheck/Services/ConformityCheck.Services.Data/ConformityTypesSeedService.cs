namespace ConformityCheck.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;

    public class ConformityTypesSeedService : IConformityTypesSeedService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;

        public ConformityTypesSeedService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
        }

        public async Task CreateAsync(ConformityTypeDTO conformityTypeDTO)
        {
            // if no description is provided
            if (conformityTypeDTO.Description == null)
            {
                throw new ArgumentNullException(nameof(conformityTypeDTO.Description));
            }

            // if this conformity type is already in the DB
            if (this.conformityTypesRepository.All()
                .FirstOrDefault(c => c.Description.ToUpper() == conformityTypeDTO.Description.ToUpper()) != null)
            {
                throw new ArgumentException($"Has this conformity type {nameof(conformityTypeDTO.Description)}");
            }

            ConformityType conformityType = new ConformityType
            {
                Description = conformityTypeDTO.Description.Trim(),
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }
    }
}
