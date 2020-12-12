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

    public class SuppliersSeedService : ISuppliersSeedService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;

        public SuppliersSeedService(
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

        public async Task CreateAsync(SupplierImportDTO supplierImportDTO)
        {
            var supplierEntity = this.suppliersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == supplierImportDTO.Number.Trim().ToUpper());

            if (supplierEntity == null)
            {
                var contactPersonNames = supplierImportDTO.Email.Split("@").FirstOrDefault().Split(".").ToList();
                supplierImportDTO.ContactPersonFirstName = contactPersonNames[0];
                supplierImportDTO.ContactPersonLastName = contactPersonNames[1];

                var supplierNames = supplierImportDTO.Email.Split("@").LastOrDefault().Split(".").ToList();
                if (supplierNames.Count() > 2 && supplierNames[1] != "com")
                {
                    supplierImportDTO.Name = supplierNames[1];
                }
                else
                {
                    supplierImportDTO.Name = supplierNames[0];
                }

                supplierEntity = new Supplier
                {
                    Number = supplierImportDTO.Number.Trim().ToUpper(),
                    Name = supplierImportDTO.Name.Trim().ToUpper(),
                    Email = supplierImportDTO.Email?.Trim(),
                    PhoneNumber = supplierImportDTO.PhoneNumber?.Trim(),
                    ContactPersonFirstName = supplierImportDTO.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(supplierImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = supplierImportDTO.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(supplierImportDTO.ContactPersonLastName),
                };

                await this.suppliersRepository.AddAsync(supplierEntity);

                await this.suppliersRepository.SaveChangesAsync();
            }
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
