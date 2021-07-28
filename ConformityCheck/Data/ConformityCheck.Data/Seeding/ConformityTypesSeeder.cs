namespace ConformityCheck.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class ConformityTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ConformityTypes.Any())
            {
                return;
            }

            var jsonConformityTypes = File
                .ReadAllText("../../../ConformityCheck/Data/ConformityCheck.Data/Seeding/DataFiles/ConformityTypesData.json");
            var conformityTypes = JsonSerializer.Deserialize<IEnumerable<ConformityTypeDTO>>(jsonConformityTypes);
            var conformityTypesService = serviceProvider.GetRequiredService<IConformityTypesSeedService>();

            foreach (var conformityType in conformityTypes)
            {
                try
                {
                    await conformityTypesService.CreateAsync(conformityType);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
