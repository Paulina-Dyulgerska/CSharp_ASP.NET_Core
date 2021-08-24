namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json; // using Newtonsoft.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypesRepository;
        private readonly IDistributedCache distributedCache;

        public ArticlesService(
            IDeletableEntityRepository<Article> articlesRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository,
            IDistributedCache distributedCache)
        {
            this.articlesRepository = articlesRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.articleConformityTypesRepository = articleConformityTypeRepository;
            this.distributedCache = distributedCache;
        }

        public int GetCount()
        {
            return this.articlesRepository.AllAsNoTracking().Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                return this.GetCount();
            }

            return this.articlesRepository
                .AllAsNoTracking()
                .Where(x => x.Number.Contains(searchInput.ToUpper())
                           || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.articlesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        // public int GetCountUnconfirmedByMainSupplier()
        // {
        //    var query = @"SELECT F.[Id]
        //                              ,F.[CreatedOn]
        //                              ,F.[ModifiedOn]
        //                              ,F.[IsDeleted]
        //                              ,F.[DeletedOn]
        //                              ,F.[Number]
        //                              ,F.[Description]
        //                              ,F.[UserId]
        //                         --,COUNT(*) AS ConformityTypesCount
        //                         --,COUNT(F.IsValid) as IsValid
        //                        FROM
        //                        ( SELECT A.[Id]
        //                            ,A.[CreatedOn]
        //                            ,A.[ModifiedOn]
        //                            ,A.[IsDeleted]
        //                            ,A.[DeletedOn]
        //                            ,A.[Number]
        //                            ,A.[UserId]
        //                            ,A.[Description]
        //                         ,CT.[Description] AS ConfTypeDescription
        //                         ,ASUP.SupplierId
        //                         ,ASUP.IsMainSupplier
        //                         ,CONF.IsAccepted
        //                         ,CONF.ValidityDate
        //                         ,CASE
        //                          WHEN (IsAccepted = 1 AND ValidityDate >= GETDATE()) THEN 1
        //                          ELSE NULL
        //                         END AS IsValid
        //                        FROM Articles AS A
        //                          LEFT JOIN ArticleConformityTypes AS ACT ON A.Id = ACT.ArticleId
        //                          LEFT JOIN ConformityTypes AS CT ON ACT.ConformityTypeId = CT.Id
        //                          LEFT JOIN ArticleSuppliers AS ASUP ON ASUP.ArticleId = A.Id
        //                          LEFT JOIN Conformities AS CONF ON
        //                            (CONF.ArticleId = A.Id AND
        //                            ASUP.SupplierId = CONF.SupplierId AND
        //                            CONF.ConformityTypeId = ACT.ConformityTypeId)
        //                         --Include this for check if just MAIN SUPPLIER has all confirmed:
        //                          WHERE ASUP.IsMainSupplier = 1
        //                        ) AS F
        //                        GROUP BY F.[Id]
        //                              ,F.[CreatedOn]
        //                              ,F.[ModifiedOn]
        //                              ,F.[IsDeleted]
        //                              ,F.[DeletedOn]
        //                              ,F.[Number]
        //                              ,F.[Description]
        //                              ,F.[UserId]
        //                        --check if supplier/s has/ve confirmed:
        //                        HAVING COUNT(*) = COUNT(F.IsValid)
        //                        --ORDER BY F.Number";
        //  return this.articlesRepository.ExecuteCustomQuery(query).Count();
        // }
        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.articlesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.articlesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var articles = await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage) // where T : ArticleDetailsExportModel
        {
            switch (sortOrder)
            {
                case GlobalConstants.NumberSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NumberSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNameSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNameSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNumberSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Number)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNumberSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Number)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierAllConfirmedSortParam:
                    return await this.articlesRepository.ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByMainSupplier)
                                        .OrderBy(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierAllConfirmedSortParamDesc:
                    return await this.articlesRepository.ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByMainSupplier)
                                        .OrderByDescending(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.AllSuppliersAllConfirmedSortParam:
                    return await this.articlesRepository.ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByAllSuppliers)
                                        .OrderBy(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.AllSuppliersAllConfirmedSortParamDesc:
                    return await this.articlesRepository.ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByAllSuppliers)
                                        .OrderByDescending(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    if (itemsPerPage == 12 && page == 1)
                    {
                        var entitiesCached = await this.distributedCache.GetStringAsync(GlobalConstants.Articles);

                        IEnumerable<T> entities;

                        if (entitiesCached == null)
                        {
                            entities = await this.articlesRepository
                                            .AllAsNoTracking()
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                            .To<T>()
                                            .ToListAsync();

                            await this.distributedCache.SetStringAsync(
                                GlobalConstants.Articles,
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

                    return await this.articlesRepository
                                            .AllAsNoTracking()
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                            .To<T>()
                                            .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            var entities = await this.articlesRepository.AllAsNoTracking()
                .Where(x => x.Number.Contains(searchInput.ToUpper())
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
                case GlobalConstants.NumberSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NumberSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.DescriptionSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNameSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNameSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNumberSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Number)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierNumberSortParamDesc:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ArticleSuppliers.FirstOrDefault().Supplier.Number)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierAllConfirmedSortParam:
                    return await this.articlesRepository
                                        .ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByMainSupplier)
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.MainSupplierAllConfirmedSortParamDesc:
                    return await this.articlesRepository
                                        .ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByMainSupplier)
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.AllSuppliersAllConfirmedSortParam:
                    return await this.articlesRepository
                                        .ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByAllSuppliers)
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.AllSuppliersAllConfirmedSortParamDesc:
                    return await this.articlesRepository
                                        .ExecuteCustomQuery(GlobalConstants.QueryArticlesOrderedByConfirmedByAllSuppliers)
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.IsConfirmed)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    return await this.articlesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task CreateAsync(ArticleCreateInputModel input, string userId)
        {
            var articleEntity = await this.articlesRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Number == input.Number.Trim().ToUpper());

            // TODO: make a validation attribute for this or to generate numbers automatically
            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var article = new Article
            {
                Number = input.Number.Trim().ToUpper(),
                Description = PascalCaseConverter.Convert(input.Description),
                UserId = userId,
            };

            await this.articlesRepository.AddAsync(article);

            await this.articlesRepository.SaveChangesAsync();

            if (input.SupplierId != null)
            {
                await this.AddSupplierAsync(new ArticleManageSuppliersInputModel()
                {
                    Id = article.Id,
                    SupplierId = input.SupplierId,
                });
            }

            if (input.ConformityTypes.Any())
            {
                await this.AddConformityTypesAsync(article, input.ConformityTypes);
            }

            // TODO: products, substances to be added too.
        }

        public async Task EditAsync(ArticleEditInputModel input, string userId)
        {
            var articleEntity = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (input.Description != null)
            {
                articleEntity.Description = PascalCaseConverter.Convert(input.Description);
                articleEntity.UserId = userId;
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id, string userId)
        {
            var articleEntity = await this.articlesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            articleEntity.UserId = userId;
            this.articlesRepository.Delete(articleEntity);

            var articleSuppliersEntities = await this.articleSuppliersRepository
                .All()
                .Where(x => x.ArticleId == id)
                .ToListAsync();

            foreach (var articleSuppliersEntity in articleSuppliersEntities)
            {
                this.articleSuppliersRepository.Delete(articleSuppliersEntity);
            }

            var articleConformityTypesEntities = await this.articleConformityTypesRepository
                .All()
                .Where(x => x.ArticleId == id)
                .ToListAsync();

            foreach (var articleConformityTypesEntity in articleConformityTypesEntities)
            {
                this.articleConformityTypesRepository.Delete(articleConformityTypesEntity);
            }

            var articleConformitiesEntities = await this.conformitiesRepository
                .All()
                .Where(x => x.ArticleId == id)
                .ToListAsync();

            foreach (var articleConformitiesEntity in articleConformitiesEntities)
            {
                this.conformitiesRepository.Delete(articleConformitiesEntity);
            }

            return await this.articlesRepository.SaveChangesAsync();
        }

        public async Task AddSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleSuppliers = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(a => a.ArticleId == input.Id)
                .ToListAsync();

            var initialNumberOfSuppliers = articleSuppliers.Count();

            if (articleSuppliers.Any(x => x.SupplierId == input.SupplierId))
            {
                // The supplier is already asigned to this article.
                return;
            }

            var articleSupplierEntity = new ArticleSupplier
            {
                ArticleId = input.Id,
                SupplierId = input.SupplierId,
            };

            await this.articleSuppliersRepository.AddAsync(articleSupplierEntity);

            if (initialNumberOfSuppliers == 0)
            {
                articleSupplierEntity.IsMainSupplier = true;
            }

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task ChangeMainSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleSupplierEntities = await this.articleSuppliersRepository
                .All()
                .Where(x => x.ArticleId == input.Id)
                .ToListAsync();

            foreach (var entity in articleSupplierEntities)
            {
                if (entity.SupplierId == input.SupplierId)
                {
                    entity.IsMainSupplier = true;
                }
                else
                {
                    entity.IsMainSupplier = false;
                }
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task RemoveSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleSupplierEntity = await this.articleSuppliersRepository.All()
                .FirstOrDefaultAsync(x => x.ArticleId == input.Id && x.SupplierId == input.SupplierId);

            this.articleSuppliersRepository.Delete(articleSupplierEntity);

            var articleConformitiesEntities = await this.conformitiesRepository
                                                          .All()
                                                          .Where(x => x.ArticleId == input.Id
                                                                   && x.SupplierId == input.SupplierId)
                                                          .ToListAsync();

            foreach (var articleConformitiesEntity in articleConformitiesEntities)
            {
                this.conformitiesRepository.Delete(articleConformitiesEntity);
            }

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task AddConformityTypeAsync(ArticleManageConformityTypesInputModel input)
        {
            await this.AddConformityTypesAsync(
                new Article { Id = input.Id },
                new int[] { input.ConformityTypeId });
        }

        public async Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes)
        {
            foreach (var conformityType in conformityTypes)
            {
                var articleConformityType = await this.articleConformityTypesRepository
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(x => x.ArticleId == article.Id
                                            && x.ConformityTypeId == conformityType);

                if (articleConformityType != null)
                {
                    // This conformity type is already asigned to the article
                    continue;
                }

                await this.articleConformityTypesRepository.AddAsync(new ArticleConformityType
                {
                    ArticleId = article.Id,
                    ConformityTypeId = conformityType,
                });
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task RemoveConformityTypesAsync(ArticleManageConformityTypesInputModel input)
        {
            var articleConformityTypeEntity = await this.articleConformityTypesRepository
                .All()
                .FirstOrDefaultAsync(x => x.ArticleId == input.Id && x.ConformityTypeId == input.ConformityTypeId);

            this.articleConformityTypesRepository.Delete(articleConformityTypeEntity);

            await this.articleConformityTypesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetSuppliersByIdAsync<T>(string id)
        {
            return await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == id)
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypeExportModel>>
            GetConformityTypesByIdAndSupplierAsync(string articleId, string supplierId)
        {
            var entities = await this.articleConformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .OrderBy(x => x.ConformityTypeId)
                .To<ConformityTypeExportModel>()
                .ToListAsync();

            foreach (var entity in entities)
            {
                var hasConformityEntity = await this.conformitiesRepository
                    .AllAsNoTracking()
                    .AnyAsync(x => x.ArticleId == articleId
                                && x.SupplierId == supplierId
                                && x.ConformityTypeId == entity.Id
                                && x.IsAccepted
                                && x.ValidityDate >= DateTime.UtcNow);

                if (hasConformityEntity)
                {
                    entity.SupplierConfirmed = true;
                }
            }

            return entities;
        }
    }
}
