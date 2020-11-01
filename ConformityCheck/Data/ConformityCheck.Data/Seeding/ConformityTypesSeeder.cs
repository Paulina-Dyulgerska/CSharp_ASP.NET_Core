namespace ConformityCheck.Data.Seeding
{
    using ConformityCheck.Services;
    using ConformityCheck.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ConformityTypesSeeder : ISeeder
    {

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            // Add conformity types:
            //IConformityTypeService conformityTypeService = new ConformityTypeService(dbContext);
            var jsonConformityTypes = File.ReadAllText("/DataFiles/ConformityTypesData.json");
            var conformityTypes = JsonSerializer.Deserialize<IEnumerable<ConformityTypeDTO>>(jsonConformityTypes);

            foreach (var conformityType in conformityTypes)
            {
                try
                {
                    //await conformityTypeService.Create(conformityType);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
