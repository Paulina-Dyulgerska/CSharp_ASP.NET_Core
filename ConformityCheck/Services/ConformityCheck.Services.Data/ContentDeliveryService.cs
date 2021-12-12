namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class ContentDeliveryService : IContentDeliveryService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<Substance> substancesRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IServiceProvider serviceProvider;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ContentDeliveryService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Substance> substancesRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IServiceProvider serviceProvider)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.productsRepository = productsRepository;
            this.substancesRepository = substancesRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.serviceProvider = serviceProvider;
            this.userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this.roleManager = this.serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
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

        public async Task<IEnumerable<T>> GetAllRolesAsync<T>()
        {
            return await this.roleManager.Roles.OrderBy(x => x.Name).To<T>().ToListAsync();
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
