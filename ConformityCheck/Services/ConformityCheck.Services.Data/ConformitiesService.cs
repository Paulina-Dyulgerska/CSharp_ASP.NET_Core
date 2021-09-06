namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;

    public class ConformitiesService : IConformitiesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;
        private readonly IDistributedCache distributedCache;
        private readonly BlobServiceClient blobServiceClient;

        public ConformitiesService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository,
            IDistributedCache distributedCache,
            BlobServiceClient blobServiceClient)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
            this.distributedCache = distributedCache;
            this.blobServiceClient = blobServiceClient;
        }

        public int GetCount()
        {
            return this.conformitiesRepository.AllAsNoTracking().Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                return this.GetCount();
            }

            return this.conformitiesRepository
                .AllAsNoTracking()
                .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.conformitiesRepository
                    .AllAsNoTracking()
                    .Where(x => x.Id == id)
                    .To<T>()
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformitiesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformitiesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var conformities = await this.conformitiesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return conformities;
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage = 12)
        {
            // var conformities = await this.GetAllAsNoTrackingOrderedAsync<T>();
            // return conformities.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            switch (sortOrder)
            {
                case GlobalConstants.ArticleNumberSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Article.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleNumberSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Article.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleDescriptionSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Article.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleDescriptionSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Article.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNumberSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Supplier.Number)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNumberSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Supplier.Number)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNameSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Supplier.Name)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNameSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Supplier.Name)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ConformityTypeDescriptionSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ConformityType.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ConformityTypeDescriptionSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ConformityType.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsAcceptedSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.IsAccepted)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsAcceptedSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.IsAccepted)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsValidSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.IsAccepted && x.ValidityDate > DateTime.UtcNow.Date)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsValidSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.IsAccepted && x.ValidityDate > DateTime.UtcNow.Date)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case GlobalConstants.CreatedOnSortParamDesc: show last created first
                default:
                    if (itemsPerPage == 12 && page == 1)
                    {
                        var entitiesCached = await this.distributedCache.GetStringAsync(GlobalConstants.Conformities);

                        IEnumerable<T> entities;

                        if (entitiesCached == null)
                        {
                            entities = await this.conformitiesRepository
                                            .AllAsNoTracking()
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                            .To<T>()
                                            .ToListAsync();

                            await this.distributedCache.SetStringAsync(
                                GlobalConstants.Conformities,
                                JsonSerializer.Serialize(entities), // JsonConvert.SerializeObject(entities),
                                new DistributedCacheEntryOptions
                                {
                                    SlidingExpiration = TimeSpan.FromSeconds(300),
                                });
                        }
                        else
                        {
                            // entities = JsonConvert.DeserializeObject<IEnumerable<T>>(entitiesCached);
                            entities = JsonSerializer.Deserialize<IEnumerable<T>>(entitiesCached);
                        }

                        return entities;
                    }

                    return await this.conformitiesRepository
                                            .AllAsNoTracking()
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                            .To<T>()
                                            .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            var entities = await this.conformitiesRepository.AllAsNoTracking()
                .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                .To<T>()
                .ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(string searchInput, string sortOrder, int page, int itemsPerPage)
        {
            switch (sortOrder)
            {
                case GlobalConstants.ArticleNumberSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Article.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleNumberSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Article.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleDescriptionSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Article.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticleDescriptionSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Article.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNumberSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Supplier.Number)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNumberSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Supplier.Number)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNameSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Supplier.Name)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.SupplierNameSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Supplier.Name)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ConformityTypeDescriptionSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ConformityType.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ConformityTypeDescriptionSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ConformityType.Description)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsAcceptedSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.IsAccepted)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsAcceptedSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.IsAccepted)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsValidSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.IsAccepted && x.ValidityDate > DateTime.UtcNow.Date)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IsValidSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.IsAccepted && x.ValidityDate > DateTime.UtcNow.Date)
                                        .ThenBy(x => x.Article.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case GlobalConstants.CreatedOnSortParamDesc: show last created first
                default:
                    return await this.conformitiesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Article.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Article.Description.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Number.ToUpper().Contains(searchInput.ToUpper())
                                           || x.Supplier.Name.ToUpper().Contains(searchInput.ToUpper())
                                           || x.ConformityType.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task<ConformityCreateInputModel> GetForCreateAsync(ConformityGetInputModel input)
        {
            var articleEntity = await this.articlesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.ArticleId)
                .FirstOrDefaultAsync();
            var conformityTypeEntity = await this.conformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.ConformityTypeId)
                .FirstOrDefaultAsync();
            var supplierEntity = await this.suppliersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.SupplierId)
                .FirstOrDefaultAsync();

            var entity = new ConformityCreateInputModel
            {
                ArticleId = articleEntity.Id,
                ArticleNumber = articleEntity.Number,
                ArticleDescription = articleEntity.Description,
                ConformityTypeId = conformityTypeEntity.Id,
                ConformityTypeDescription = conformityTypeEntity.Description,
                SupplierId = supplierEntity.Id,
                SupplierName = supplierEntity.Name,
                SupplierNumber = supplierEntity.Number,
                CallerViewName = input.CallerViewName,
            };

            return entity;
        }

        public async Task CreateAsync(ConformityCreateInputModel input, string userId, string conformityFilePath)
        {
            if (input.ValidForSingleArticle)
            {
                await this.AddConformityToAnArticleAsync(input, userId, conformityFilePath);
                return;
            }

            var articleSuppliersEntities = await this.articleSuppliersRepository
                    .All()
                    .Where(x => x.SupplierId == input.SupplierId)
                    .ToListAsync();

            // za view componenta e towa: da se vika tq w controllera i da se prenasochva kym specialno view
            // za editvane na conformity, a ne za createvane!!!!
            foreach (var articleSupplierEntity in articleSuppliersEntities)
            {
                input.ArticleId = articleSupplierEntity.ArticleId;
                await this.AddConformityToAnArticleAsync(input, userId, conformityFilePath);
            }
        }

        public async Task EditAsync(ConformityEditInputModel input, string userId, string conformityFilePath)
        {
            // Already has such conformity to make an Edit!!!! File cannot be edited! Only delete and add new if
            // the file has to be updated!
            var conformityEntity = await this.conformitiesRepository
                                                .All()
                                                .FirstOrDefaultAsync(x => x.Id == input.Id);

            conformityEntity.IssueDate = input.IssueDate.ToUniversalTime().Date;
            conformityEntity.IsAccepted = input.IsAccepted;
            conformityEntity.AcceptanceDate = DateTime.UtcNow.Date;
            conformityEntity.ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null;
            conformityEntity.Comments = input.Comments;
            conformityEntity.UserId = userId;

            if (input.ValidityDate != null && input.IsAccepted)
            {
                conformityEntity.ValidityDate = input.ValidityDate?.ToUniversalTime().Date;
            }

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id, string userId)
        {
            var entity = await this.conformitiesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            // this.conformitiesRepository.HardDelete(entity); ?
            this.conformitiesRepository.Delete(entity);

            return await this.conformitiesRepository.SaveChangesAsync();
        }

        public async Task AddRequestDateAsync(ConformityGetInputModel input, string userId)
        {
            var currentConformity = await this.conformitiesRepository.All()
                            .Where(x => x.ArticleId == input.ArticleId
                                        && x.SupplierId == input.SupplierId
                                        && x.ConformityTypeId == input.ConformityTypeId)
                            .FirstOrDefaultAsync();

            if (currentConformity == null)
            {
                currentConformity = new Conformity
                {
                    ArticleId = input.ArticleId,
                    SupplierId = input.SupplierId,
                    ConformityTypeId = input.ConformityTypeId,
                    IssueDate = DateTime.UtcNow.Date,
                    IsAccepted = false,
                    UserId = userId,
                };

                await this.conformitiesRepository.AddAsync(currentConformity);
            }

            currentConformity.RequestDate = DateTime.UtcNow;

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public ConformityFileExportModel GetConformityFileFromLocalStorage(string conformityFileUrl)
        {
            string filePath = "~" + conformityFileUrl;

            var conformityFile = new ConformityFileExportModel
            {
                FilePath = filePath,
            };

            return conformityFile;
        }

        public async Task<ConformityFileExportModel> GetConformityFileFromBlobStorage(string conformityFileUrl)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants
                                                                                .AzureStorageBlobContainerName);
            var conformityFileBlob = container.GetBlobClient(conformityFileUrl.Split('/').LastOrDefault());
            var downloadedConformityFile = await conformityFileBlob.DownloadContentAsync();

            var conformityFile = new ConformityFileExportModel
            {
                FilePath = conformityFileBlob.Uri.AbsoluteUri,
                FileBytes = downloadedConformityFile.Value.Content.ToArray(),
                FileContentType = downloadedConformityFile.Value.Details.ContentType,
            };

            return conformityFile;
        }

        private async Task AddConformityToAnArticleAsync(
           ConformityCreateInputModel input,
           string userId,
           string conformityFilePath)
        {
            var currentConformity = await this.conformitiesRepository.All()
                .Where(x => x.ArticleId == input.ArticleId && x.SupplierId == input.SupplierId && x.ConformityTypeId == input.ConformityTypeId)
                .FirstOrDefaultAsync();

            if (currentConformity != null)
            {
                await this.DeleteAsync(currentConformity.Id, userId);
            }

            var articleConformityType = this.articleConformityTypeRepository
                                    .All()
                                    .Any(x => x.ConformityTypeId == input.ConformityTypeId
                                                    && x.ArticleId == input.ArticleId);

            // Add conformity type to article if not assigned:
            if (!articleConformityType)
            {
                await this.articleConformityTypeRepository.AddAsync(new ArticleConformityType
                {
                    ConformityTypeId = input.ConformityTypeId,
                    ArticleId = input.ArticleId,
                });
            }

            var conformityEntity = new Conformity
            {
                ArticleId = input.ArticleId,
                SupplierId = input.SupplierId,
                ConformityTypeId = input.ConformityTypeId,
                IssueDate = input.IssueDate.ToUniversalTime().Date,
                IsAccepted = input.IsAccepted,
                AcceptanceDate = DateTime.UtcNow.Date,
                ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null,
                Comments = input.Comments,
                UserId = userId,
            };

            if (input.ValidityDate != null && input.IsAccepted)
            {
                conformityEntity.ValidityDate = input.ValidityDate?.ToUniversalTime().Date;
            }

            await this.conformitiesRepository.AddAsync(conformityEntity);

            // upload to local wwwroot/conformities dir:
            Directory.CreateDirectory($"{conformityFilePath}/conformities/");
            var extension = Path.GetExtension(input.InputFile.FileName).ToLower().TrimStart('.');
            conformityEntity.FileExtension = extension;
            var physicalPath = $"{conformityFilePath}/conformities/{conformityEntity.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await input.InputFile.CopyToAsync(fileStream);

            //// upload to Azure Blob variant 1:
            //// await this.blobServiceClient.CreateBlobContainerAsync(GlobalConstants.AzureStorageBlobContainerName);
            // var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants.AzureStorageBlobContainerName);
            // var blobClient = container.GetBlobClient($"{conformityEntity.Id}.{extension}");
            // byte[] destinationFile;
            // using (var memoryStream = new MemoryStream())
            // {
            //     await input.InputFile.CopyToAsync(memoryStream);
            //     destinationFile = memoryStream.ToArray();
            //     memoryStream.Position = 0;
            //     await blobClient.UploadAsync(memoryStream);
            //     conformityEntity.RemoteFileUrl = blobClient.Uri.AbsoluteUri;
            // }

            // upload to Azure Blob variant 2:
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants.AzureStorageBlobContainerName);
            var blobClient = container.GetBlobClient($"{conformityEntity.Id}.{extension}");

            // fileStream pointer must be returned at its 0 byte, because it is at the last byte at the moment:
            fileStream.Position = 0;
            await blobClient.UploadAsync(fileStream);
            conformityEntity.RemoteFileUrl = blobClient.Uri.AbsoluteUri;

            await this.conformitiesRepository.SaveChangesAsync();
        }
    }
}
