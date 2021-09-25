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

    public class SuppliersSeedService : ISuppliersSeedService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IServiceProvider serviceProvider;

        public SuppliersSeedService(
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

        public async Task CreateAsync(SupplierImportDTO supplierImportDTO)
        {
            var userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);

            var supplierEntity = this.suppliersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == supplierImportDTO.Number.Trim().ToUpper());

            if (supplierEntity == null)
            {
                var contactPersonNames = supplierImportDTO.Email.Split("@").FirstOrDefault().Split(".").ToList();
                supplierImportDTO.ContactPersonFirstName = contactPersonNames[0];
                supplierImportDTO.ContactPersonLastName = contactPersonNames.Count > 1 ?
                    contactPersonNames[1] : string.Empty;

                var supplierNames = supplierImportDTO.Email.Split("@").LastOrDefault().Split(".").ToList();
                if (supplierNames.Count() > 2 && supplierNames[1] != "com")
                {
                    supplierImportDTO.Name = supplierNames[1];
                }
                else
                {
                    supplierImportDTO.Name = supplierNames[0];
                }

                var random = new Random();
                var numb = random.Next(100000, 999999);
                var hasSuchNumber = this.suppliersRepository.AllAsNoTrackingWithDeleted()
                    .Any(x => x.Number == numb.ToString());
                while (hasSuchNumber)
                {
                    numb = random.Next(100000, 999999);
                    hasSuchNumber = this.suppliersRepository.AllAsNoTrackingWithDeleted()
                    .Any(x => x.Number == numb.ToString());
                }

                supplierEntity = new Supplier
                {
                    // Email = supplierImportDTO.Email.Trim(),
                    Email = "paylina_st@yahoo.com",
                    Number = supplierImportDTO.Number.Trim().ToUpper(),
                    Name = supplierImportDTO.Name.Trim().ToUpper(),
                    PhoneNumber = supplierImportDTO.PhoneNumber?.Trim(),
                    ContactPersonFirstName = supplierImportDTO.ContactPersonFirstName == null ? null :
                            PascalCaseConverter.Convert(supplierImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = supplierImportDTO.ContactPersonLastName == null ? null :
                            PascalCaseConverter.Convert(supplierImportDTO.ContactPersonLastName),
                    User = adminUsers.FirstOrDefault(),
                };

                await this.suppliersRepository.AddAsync(supplierEntity);

                await this.suppliersRepository.SaveChangesAsync();
            }
        }
    }
}
