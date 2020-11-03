namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Models;

    public class ConformityTypesSeeder : ISeeder
    {

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {

            if (!dbContext.ConformityTypes.Any())
            {
                // Add conformity types:
                //IConformityTypeService conformityTypeService = new ConformityTypeService(dbContext);
                var jsonConformityTypes = File.ReadAllText
                ("../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ConformityTypesData.json");
                //var conformityTypes = JsonSerializer.Deserialize<IEnumerable<ConformityTypeDTO>>(jsonConformityTypes);
                var conformityTypes = JsonSerializer.Deserialize<IEnumerable<ConformityType>>(jsonConformityTypes);

                foreach (var conformityType in conformityTypes)
                {
                    try
                    {
                        await dbContext.ConformityTypes.AddAsync(conformityType);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
