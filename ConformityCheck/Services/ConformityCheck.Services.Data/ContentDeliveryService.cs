namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ContentDeliveryService : IContentDeliveryService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<Substance> substancesRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;

        public ContentDeliveryService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Substance> substancesRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.productsRepository = productsRepository;
            this.substancesRepository = substancesRepository;
            this.conformityTypesRepository = conformityTypesRepository;
        }

        public async Task<IEnumerable<T>> GetAllArticlesAsync<T>()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetLastCreatedArticlesAsync<T>()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllSuppliersAsync<T>()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetLastCreatedSuppliersAsync<T>()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Name)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllConformityTypesAsync<T>()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetLastCreatedConformityTypesAsync<T>()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Description)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllProductsAsync<T>()
        {
            return await this.productsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetLastCreatedProductsAsync<T>()
        {
            return await this.productsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllSubstancesAsync<T>()
        {
            return await this.substancesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.CASNumber)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetLastCreatedSubstancesAsync<T>()
        {
            return await this.substancesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.CASNumber)
                .To<T>()
                .ToListAsync();
        }
    }
}
