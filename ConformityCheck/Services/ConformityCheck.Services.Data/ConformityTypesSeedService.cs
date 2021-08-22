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

    public class ConformityTypesSeedService : IConformityTypesSeedService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IServiceProvider serviceProvider;

        public ConformityTypesSeedService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IServiceProvider serviceProvider)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.serviceProvider = serviceProvider;
        }

        public async Task CreateAsync(ConformityTypeDTO conformityTypeDTO)
        {
            var userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);

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
                User = adminUsers.FirstOrDefault(),
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }
    }
}
