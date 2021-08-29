namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Data.Models;
    using CsvHelper;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class SuppliersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Suppliers.Any())
            {
                return;
            }

            // CSV import:
            var fileCsvToCSharp = "../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/Suppliers.csv";
            var suppliersService = serviceProvider.GetRequiredService<ISuppliersSeedService>();
            var logger = serviceProvider.GetRequiredService<ILogger<ISuppliersSeedService>>();

            using (CsvReader reader = new CsvReader(new StreamReader(fileCsvToCSharp), CultureInfo.InvariantCulture))
            {
                var suppliersImportDTOs = reader.GetRecords<SupplierImportDTO>();
                foreach (var suppliersImportDTO in suppliersImportDTOs)
                {
                    try
                    {
                        await suppliersService.CreateAsync(suppliersImportDTO);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Adding supplier {suppliersImportDTO.Number} - {suppliersImportDTO.Name} throws and Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
