﻿namespace ConformityCheck.Services.Data
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using Microsoft.Extensions.Caching.Distributed;

    public class GetCountsService : IGetCountsService
    {
        private const string Counts = "Counts";
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Substance> substancesRepository;
        private readonly IDeletableEntityRepository<RegulationList> regulationListsRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDistributedCache distributedCache;

        public GetCountsService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Substance> substancesRepository,
            IDeletableEntityRepository<RegulationList> regulationListsRepository,
            IDistributedCache distributedCache)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.substancesRepository = substancesRepository;
            this.regulationListsRepository = regulationListsRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.productsRepository = productsRepository;
            this.distributedCache = distributedCache;
        }

        public async Task<CountsDto> GetCounts()
        {
            var countsCached = await this.distributedCache.GetStringAsync(Counts);

            CountsDto counts;

            if (countsCached == null)
            {
                counts = new CountsDto
                {
                    Articles = this.articlesRepository.AllAsNoTracking().Count(),
                    Suppliers = this.suppliersRepository.AllAsNoTracking().Count(),
                    Products = this.productsRepository.AllAsNoTracking().Count(),
                    Conformities = this.conformitiesRepository.AllAsNoTracking().Count(),
                    ConformityTypes = this.conformityTypesRepository.AllAsNoTracking().Count(),
                    Substances = this.substancesRepository.AllAsNoTracking().Count(),
                    RegulationLists = this.regulationListsRepository.AllAsNoTracking().Count(),
                };

                await this.distributedCache.SetStringAsync(
                    Counts,
                    JsonSerializer.Serialize(counts),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(300),
                    });
            }
            else
            {
                counts = JsonSerializer.Deserialize<CountsDto>(countsCached);
            }

            return counts;
        }
    }
}
