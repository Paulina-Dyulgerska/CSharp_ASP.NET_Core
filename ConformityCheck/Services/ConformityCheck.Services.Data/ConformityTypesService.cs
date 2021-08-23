namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;

    public class ConformityTypesService : IConformityTypesService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDistributedCache distributedCache;

        public ConformityTypesService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDistributedCache distributedCache)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.distributedCache = distributedCache;
        }

        public int GetCount()
        {
            return this.conformityTypesRepository.AllAsNoTracking().Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                return this.GetCount();
            }

            return this.conformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                           || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.conformityTypesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformityTypesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformityTypesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var conformityTypes = await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Description)
                .ThenByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return conformityTypes;
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(
            string sortOrder,
            int page,
            int itemsPerPage = 12)
        {
            // var conformityTypes = await this.GetAllAsNoTrackingOrderedAsync<T>();
            // return conformityTypes.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            switch (sortOrder)
            {
                case GlobalConstants.IdSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IdSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ModifiedOnSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ModifiedOnSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                case GlobalConstants.CreatedOnSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case GlobalConstants.CreatedOnSortParamDesc: show last created first
                default:
                    if (itemsPerPage == 12 && page == 1)
                    {
                        var entitiesCached = await this.distributedCache.GetStringAsync(GlobalConstants.ConformityTypes);

                        IEnumerable<T> entities;

                        if (entitiesCached == null)
                        {
                            // entities = await this.conformityTypesRepository
                            //                .AllAsNoTracking()
                            //                .OrderByDescending(x => x.CreatedOn)
                            //                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                            //                .To<T>()
                            //                .ToListAsync();
                            // await this.distributedCache.SetStringAsync(
                            //    GlobalConstants.ConformityTypes,
                            //    JsonSerializer.Serialize(entities), // JsonConvert.SerializeObject(entities),
                            //    new DistributedCacheEntryOptions
                            //    {
                            //        SlidingExpiration = TimeSpan.FromSeconds(300),
                            //    });
                            entities = await this.UpdateCache<T>();
                        }
                        else
                        {
                            // entities = JsonConvert.DeserializeObject<IEnumerable<T>>(entitiesCached);
                            entities = JsonSerializer.Deserialize<IEnumerable<T>>(entitiesCached);
                        }

                        return entities;
                    }

                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            var entities = await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .To<T>()
                                        .ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(
            string searchInput,
            string sortOrder,
            int page,
            int itemsPerPage)
        {
            switch (sortOrder)
            {
                case GlobalConstants.IdSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.IdSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ModifiedOnSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ModifiedOnSortParamDesc:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                case GlobalConstants.CreatedOnSortParam:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case GlobalConstants.CreatedOnSortParamDesc: show last created first
                default:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task CreateAsync(ConformityTypeCreateInputModel input, string userId)
        {
            var conformityType = new ConformityType
            {
                // Description = WebUtility.HtmlEncode(input.Description.ToUpper()),
                Description = input.Description.ToUpper(),
                UserId = userId,
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();

            // update cache
            await this.UpdateCache<ConformityTypeExportModel>();
        }

        public async Task EditAsync(ConformityTypeEditInputModel input, string userId)
        {
            // if this conformity type is not in the DB
            var entity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == input.Id);

            entity.Description = input.Description.ToUpper();
            entity.UserId = userId;

            await this.conformityTypesRepository.SaveChangesAsync();

            // update cache
            await this.UpdateCache<ConformityTypeExportModel>();
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var entity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            entity.UserId = userId;

            // if this conformity type is assigned to article or product in the DB, no delete is possible - the
            // validation attribute ConformityTypeUsageAttribute is checking this.
            // if (this.conformitiesRepository.All().Any(ac => ac.ConformityTypeId == id))
            // {
            //    throw new ArgumentException($"Cannot delete conformity type with articles assigned to it.");
            // }
            this.conformityTypesRepository.Delete(entity);

            // update cache
            await this.UpdateCache<ConformityTypeExportModel>();

            return await this.conformityTypesRepository.SaveChangesAsync();
        }

        private async Task<IEnumerable<T>> UpdateCache<T>()
        {
            var entities = await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip(0).Take(12)
                .To<T>()
                .ToListAsync();

            await this.distributedCache.SetStringAsync(
                GlobalConstants.ConformityTypes,
                JsonSerializer.Serialize(entities), // JsonConvert.SerializeObject(entities),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(60 * 60 * 24 * 365),
                });

            return entities;
        }
    }
}
