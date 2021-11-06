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
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDistributedCache distributedCache;

        public SuppliersService(
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IRepository<ArticleSupplier> articleSupplierRepository,
            IDistributedCache distributedCache)
        {
            this.suppliersRepository = suppliersRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.articleSuppliersRepository = articleSupplierRepository;
            this.distributedCache = distributedCache;
        }

        public int GetCount()
        {
            return this.suppliersRepository.AllAsNoTracking().Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                return this.GetCount();
            }

            return this.suppliersRepository
                .AllAsNoTracking()
                .Where(x => x.Number.Contains(searchInput.ToUpper())
                           || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.suppliersRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.suppliersRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.suppliersRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.ArticleSuppliers.Count())
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Number)
                .ThenByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            switch (sortOrder)
            {
                case GlobalConstants.NumberSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NumberSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NameSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NameSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticlesCountSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ArticleSuppliers.Count())
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticlesCountSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ArticleSuppliers.Count())
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    if (itemsPerPage == 12 && page == 1)
                    {
                        var entitiesCached = await this.distributedCache.GetStringAsync(GlobalConstants.Suppliers);

                        IEnumerable<T> entities;

                        if (entitiesCached == null)
                        {
                            entities = await this.suppliersRepository
                                            .AllAsNoTracking()
                                            .OrderByDescending(x => x.CreatedOn)
                                            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                            .To<T>()
                                            .ToListAsync();

                            await this.distributedCache.SetStringAsync(
                                GlobalConstants.Suppliers,
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

                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            var entities = await this.suppliersRepository.AllAsNoTracking()
                .Where(x => x.Number.Contains(searchInput.ToUpper())
                        || x.Name.ToUpper().Contains(searchInput.ToUpper()))
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
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NumberSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Number)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NameSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.NameSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Name)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticlesCountSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ArticleSuppliers.Count())
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.ArticlesCountSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ArticleSuppliers.Count())
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.UserEmailSortParamDesc:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Number)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case GlobalConstants.CreatedOnSortParam:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    return await this.suppliersRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Number.Contains(searchInput.ToUpper())
                                            || x.Name.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetArticlesByIdAsync<T>(string id)
        {
            var articles = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(x => x.SupplierId == id)
                .OrderBy(x => x.Article.Number)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<SupplierArticlesDetailsExportModel> GetByIdWIthArticlesAndConformityOrderedAsPageAsync(
           string id,
           string sortOrder,
           int page,
           int itemsPerPage)
        {
            var supplier = await this.GetByIdAsync<SupplierArticlesDetailsExportModel>(id);

            switch (sortOrder)
            {
                case GlobalConstants.NumberSortParamDesc:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                                                .Where(x => x.SupplierId == id)
                                                .OrderByDescending(a => a.Article.Number)
                                                .SelectMany(x => x.Article.ArticleConformityTypes)
                                                .Skip((page - 1) * itemsPerPage)
                                                .Take(itemsPerPage)
                                                .To<ArticleConformityTypeConformitiesExportModel>()
                                                .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = this.articleSuppliersRepository.AllAsNoTracking()
                    //                          .Where(x => x.SupplierId == id)
                    //                          .OrderByDescending(a => a.Article.Number)
                    //                          .SelectMany(x => x.Article.ArticleConformityTypes)
                    //                          .Include(x => x.Article.Conformities)
                    //                          .Skip((page - 1) * itemsPerPage)
                    //                          .Take(itemsPerPage)
                    //                          .To<ArticleConformityTypeConformitiesExportModel>()
                    //                          .ToList();
                    return supplier;

                case GlobalConstants.DescriptionSortParam:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                                                .Where(x => x.SupplierId == id)
                                                .OrderByDescending(x => x.Article.Description)
                                                .SelectMany(x => x.Article.ArticleConformityTypes)
                                                .Skip((page - 1) * itemsPerPage)
                                                .Take(itemsPerPage)
                                                .To<ArticleConformityTypeConformitiesExportModel>()
                                                .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //                          .Where(x => x.SupplierId == id)
                    //                          .SelectMany(x => x.Article.ArticleConformityTypes)
                    //                          .Include(x => x.Article.Conformities)
                    //                          .Include(x => x.ConformityType)
                    //                          .OrderByDescending(x => x.Article.Description)
                    //                          .ThenBy(a => a.Article.Number)
                    //                          .Skip((page - 1) * itemsPerPage)
                    //                          .Take(itemsPerPage)
                    //                          .To<ArticleConformityTypeConformitiesExportModel>()
                    //                          .ToListAsync();
                    return supplier;

                case GlobalConstants.DescriptionSortParamDesc:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                                                .Where(x => x.SupplierId == id)
                                                .OrderBy(x => x.Article.Description)
                                                .SelectMany(x => x.Article.ArticleConformityTypes)
                                                .Skip((page - 1) * itemsPerPage)
                                                .Take(itemsPerPage)
                                                .To<ArticleConformityTypeConformitiesExportModel>()
                                                .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //                          .Where(x => x.SupplierId == id)
                    //                          .SelectMany(x => x.Article.ArticleConformityTypes)
                    //                          .Include(x => x.Article.Conformities)
                    //                          .Include(x => x.ConformityType)
                    //                          .OrderBy(x => x.Article.Description)
                    //                          .ThenBy(a => a.Article.Number)
                    //                          .Skip((page - 1) * itemsPerPage)
                    //                          .Take(itemsPerPage)
                    //                          .To<ArticleConformityTypeConformitiesExportModel>()
                    //                          .ToListAsync();
                    return supplier;

                case GlobalConstants.ConformityTypeSortParam:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                                                .Where(x => x.SupplierId == id)
                                                .SelectMany(x => x.Article.ArticleConformityTypes)
                                                .To<ArticleConformityTypeConformitiesExportModel>()
                                                .OrderByDescending(x => x.ConformityTypeDescription)
                                                .Skip((page - 1) * itemsPerPage)
                                                .Take(itemsPerPage)
                                                .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //                            .Where(x => x.SupplierId == id)
                    //                            .SelectMany(x => x.Article.ArticleConformityTypes)
                    //                            .Include(x => x.Article.Conformities)
                    //                            .Include(x => x.ConformityType)
                    //                            .OrderByDescending(x => x.ConformityType.Description)
                    //                            .ThenBy(a => a.Article.Number)
                    //                            .Skip((page - 1) * itemsPerPage)
                    //                            .Take(itemsPerPage)
                    //                            .To<ArticleConformityTypeConformitiesExportModel>()
                    //                            .ToListAsync();
                    return supplier;

                case GlobalConstants.ConformityTypeSortParamDesc:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .OrderBy(x => x.ConformityTypeDescription)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage)
                        .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //   .Where(x => x.SupplierId == id)
                    //   .SelectMany(x => x.Article.ArticleConformityTypes)
                    //   .Include(x => x.Article.Conformities)
                    //   .Include(x => x.ConformityType)
                    //   .OrderBy(x => x.ConformityType.Description)
                    //   .ThenBy(a => a.Article.Number)
                    //   .Skip((page - 1) * itemsPerPage)
                    //   .Take(itemsPerPage)
                    //   .To<ArticleConformityTypeConformitiesExportModel>()
                    //   .ToListAsync();
                    return supplier;

                case GlobalConstants.HasConformitySortParam:
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateHasConformitySortParam = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateHasConformitySortParam, page, itemsPerPage);
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .OrderBy(x => x.ConformityId)
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToListAsync();
                    return supplier;

                case GlobalConstants.HasConformitySortParamDesc:
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateHasConformitySortParamDesc = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateHasConformitySortParamDesc, page, itemsPerPage, reverse: true);
                    // return supplier;
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .OrderByDescending(x => x.ConformityId)
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToListAsync();
                    return supplier;

                case GlobalConstants.AcceptedConformitySortParam:
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateAcceptedConformitySortParam = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId && x.IsAccepted;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateAcceptedConformitySortParam, page, itemsPerPage);
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .OrderBy(x => x.ConformityIsAccepted)
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToListAsync();
                    return supplier;

                case GlobalConstants.AcceptedConformitySortParamDesc:
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateAcceptedConformitySortParamDesc = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId && x.IsAccepted;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateAcceptedConformitySortParamDesc, page, itemsPerPage, reverse: true);
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .OrderByDescending(x => x.ConformityIsAccepted)
                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                        .ToListAsync();
                    return supplier;

                case GlobalConstants.ValidConformitySortParam:
                    //// variant 1: the query with the help method
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateValidConformitySortParam = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId && x.IsAccepted && x.ValidityDate >= DateTime.UtcNow;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateValidConformitySortParam, page, itemsPerPage, reverse: true);

                    //// variant 2
                    //// this is graceful but cannot be translated from EF, therefore we use the above query with a help method:
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //   .Where(x => x.SupplierId == id)
                    //   .SelectMany(x => x.Article.ArticleConformityTypes)
                    //   .To<ArticleConformityTypeConformitiesExportModel>()
                    //   .OrderByDescending(x => x.ConformityIsValid)
                    //   .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                    //   .ToListAsync();

                    // variant 3
                    // this is the fastest way - take all articles and order then here, because EF cannot do this
                    // fast and sometimes throws Exception for too long query time
                    var articlesValidConformitySortParam = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .ToListAsync();
                    supplier.Articles = articlesValidConformitySortParam
                        .OrderBy(x => x.ConformityIsValid)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.ValidConformitySortParamDesc:
                    //// variant 1 - the query with the help method
                    // Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicateValidConformitySortParamDesc = (x, article) =>
                    // {
                    //     return x.ConformityTypeId == article.ConformityTypeId && x.IsAccepted && x.ValidityDate >= DateTime.UtcNow;
                    // };
                    // supplier.Articles = await this.ExtractArticlesBySupplierIdAndPredicate(id, predicateValidConformitySortParamDesc, page, itemsPerPage);

                    //// variant 2
                    //// this is graceful but cannot be translated from EF, therefore we use the above query with a help method:
                    // supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                    //    .Where(x => x.SupplierId == id)
                    //    .SelectMany(x => x.Article.ArticleConformityTypes)
                    //    .To<ArticleConformityTypeConformitiesExportModel>()
                    //    .OrderBy(x => x.ConformityIsValid)
                    //    .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                    //    .ToListAsync();

                    // variant 3
                    // this is the fastest way - take all articles and order then here, because EF cannot do this
                    // fast and sometimes throws Exception for too long query time
                    var articlesValidConformitySortParamDesc = await this.articleSuppliersRepository.AllAsNoTracking()
                        .Where(x => x.SupplierId == id)
                        .SelectMany(x => x.Article.ArticleConformityTypes)
                        .To<ArticleConformityTypeConformitiesExportModel>()
                        .ToListAsync();
                    supplier.Articles = articlesValidConformitySortParamDesc
                        .OrderByDescending(x => x.ConformityIsValid)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);

                    return supplier;

                // case "NumberSortParam":
                default:
                    supplier.Articles = await this.articleSuppliersRepository.AllAsNoTracking()
                            .Where(x => x.SupplierId == id)
                            .OrderBy(a => a.Article.Number)
                            .SelectMany(x => x.Article.ArticleConformityTypes)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .To<ArticleConformityTypeConformitiesExportModel>()
                            .ToListAsync();

                    //// equal in speed performance with the above query, but the above one is graceful
                    // supplier.Articles = this.articleSuppliersRepository.AllAsNoTracking()
                    //        .Where(x => x.SupplierId == id)
                    //        .OrderBy(a => a.Article.Number)
                    //        .SelectMany(x => x.Article.ArticleConformityTypes)
                    //        .Include(x => x.Article.Conformities)
                    //        .Skip((page - 1) * itemsPerPage)
                    //        .Take(itemsPerPage)
                    //        .To<ArticleConformityTypeConformitiesExportModel>()
                    //        .ToList();
                    return supplier;

                    // variant 1 - slower than current:
                    // var supplier = await this.GetByIdAsync<SupplierArticlesDetailsExportModel>(id);
                    // switch (sortOrder)
                    // {
                    //     case GlobalConstants.NumberSortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ArticleNumber)
                    //             .ThenBy(a => a.ConformityTypeDescription)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.DescriptionSortParam:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ArticleDescription)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.DescriptionSortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ArticleDescription)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.ConformityTypeSortParam:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ConformityTypeDescription)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.ConformityTypeSortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ConformityTypeDescription)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.HasConformitySortParam:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ArticleConformity?.Id)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.HasConformitySortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ArticleConformity?.Id)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.AcceptedConformitySortParam:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ArticleConformity?.IsAccepted)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.AcceptedConformitySortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ArticleConformity?.IsAccepted)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.ValidConformitySortParam:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ArticleConformity?.IsValid)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     case GlobalConstants.ValidConformitySortParamDesc:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderByDescending(a => a.ArticleConformity?.IsValid)
                    //             .ThenBy(a => a.ArticleNumber)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
                    //     // case "number":
                    //     default:
                    //         supplier.Articles = supplier.Articles
                    //             .OrderBy(a => a.ArticleNumber)
                    //             .ThenBy(a => a.ConformityTypeDescription)
                    //             .Skip((page - 1) * itemsPerPage)
                    //             .Take(itemsPerPage);
                    //         return supplier;
            }
        }

        // not needed since variant 3 is used! This is needed only for variant 1:
        private async Task<IEnumerable<ArticleConformityTypeConformitiesExportModel>> ExtractArticlesBySupplierIdAndPredicate(
            string id,
            Func<ConformityBaseExportModel, ArticleConformityTypeConformitiesExportModel, bool> predicate,
            int page,
            int itemsPerPage,
            bool reverse = false)
        {
            var articles = await this.articleSuppliersRepository.AllAsNoTracking()
                                      .Where(x => x.SupplierId == id)
                                      .SelectMany(x => x.Article.ArticleConformityTypes)
                                      .Include(x => x.Article.Conformities)
                                      .To<ArticleConformityTypeConformitiesExportModel>()
                                      .ToListAsync();

            var first = new SortedDictionary<string, ArticleConformityTypeConformitiesExportModel>();
            var second = new SortedDictionary<string, ArticleConformityTypeConformitiesExportModel>();

            // I can create instead of first and second, just one sorted dictionary with key true or false
            // and present the data with its help!!!
            // var third = new SortedDictionary<bool, List<ArticleConformityTypeConformitiesExportModel>>();
            foreach (var article in articles)
            {
                var key = article.ArticleNumber + article.ConformityTypeDescription;

                if (article.Conformities == null)
                {
                    second.Add(key, article);
                    continue;
                }

                if (article.Conformities.Any(x => predicate(x, article)))
                {
                    first.Add(key, article);
                }
                else
                {
                    second.Add(key, article);
                }
            }

            var result = new List<ArticleConformityTypeConformitiesExportModel>();
            if (reverse)
            {
                result.AddRange(second.Values);
                result.AddRange(first.Values);
            }
            else
            {
                result.AddRange(first.Values);
                result.AddRange(second.Values);
            }

            return result.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
        }

        // public async Task<IEnumerable<ArticleConformityExportModel>>
        //     GetArticlesBySearchInputWithConformityByIdOrderedAsPageAsync(
        //                string id,
        //                string searchInput,
        //                string sortOrder,
        //                int page,
        //                int itemsPerPage)
        // {
        //     var supplier = await this.GetByIdAsync<SupplierArticlesDetailsExportModel>(id);
        //     switch (sortOrder)
        //     {
        //         case "numberDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ArticleNumber)
        //                 .ThenBy(a => a.ConformityTypeDescription)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "description":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ArticleDescription)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "descriptionDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ArticleDescription)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "conformityType":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ConformityTypeDescription)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "conformityTypeDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ConformityTypeDescription)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "hasConformity":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ArticleConformity)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "hasConformityDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ArticleConformity)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "acceptedConformity":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ArticleConformity?.IsAccepted)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "acceptedConformityDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ArticleConformity?.IsAccepted)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "validConformity":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ArticleConformity?.IsValid)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "validConformityDesc":
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderByDescending(a => a.ArticleConformity?.IsValid)
        //                 .ThenBy(a => a.ArticleNumber)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //         case "number":
        //         default:
        //             return supplier.Articles
        //                 .Where(x => x.ArticleNumber.Contains(searchInput.ToUpper())
        //                           || x.ArticleDescription.ToUpper().Contains(searchInput.ToUpper()))
        //                 .OrderBy(a => a.ArticleNumber)
        //                 .ThenBy(a => a.ConformityTypeDescription)
        //                 .Skip((page - 1) * itemsPerPage)
        //                 .Take(itemsPerPage);
        //     }
        // }

        // public async Task<SupplierArticlesDetailsExportModel> DetailsByIdAsync(string id)
        // {
        //     // SELECT*
        //     //  FROM ArticleConformityTypes AS ACONT
        //     //  LEFT JOIN ArticleSuppliers AS ASUP ON ACONT.ArticleId = ASUP.ArticleId
        //     //  JOIN Suppliers AS SUP ON ASUP.SupplierId = SUP.Id
        //     //  LEFT JOIN Conformities AS CON ON ASUP.ArticleId = CON.ArticleId
        //     //  WHERE SUP.Id = id

        // var model = await this.GetByIdAsync<SupplierArticlesDetailsExportModel>(id);

        // //// This is exporting Conformity class for ArticleConformity: not needed since the class SupplierArticlesDetailsExportModel
        //     //// is doing the article.ArticleConformity data internally:
        //     // model.Articles = await this.articleConformityTypesRepository
        //     //  .AllAsNoTracking()
        //     //  .Where(ac => ac.Article.ArticleSuppliers.Any(x => x.SupplierId == id))
        //     //  .Select(ac => new ArticleConformityExportModel
        //     //  {
        //     //      ArticleId = ac.ArticleId,
        //     //      ArticleNumber = ac.Article.Number,
        //     //      ArticleDescription = ac.Article.Description,
        //     //      ConformityTypeId = ac.ConformityTypeId,
        //     //      ConformityTypeDescription = ac.ConformityType.Description,
        //     //      ArticleConformity = ac.Article.Conformities
        //     //                                      .Where(x => x.SupplierId == id
        //     //                                      && x.ConformityTypeId == ac.ConformityTypeId)
        //     //                                      .FirstOrDefault(),
        //     //  })
        //     //  .ToListAsync();

        // //// This is exporting ConformityValidityExportModel class for ArticleConformity: not needed since the
        //     //// class SupplierArticlesDetailsExportModel is doing the article.ArticleConformity data internally:
        //     // model.Articles = await this.articleConformityTypesRepository
        //     //       .AllAsNoTracking()
        //     //       .Where(ac => ac.Article.ArticleSuppliers.Any(x => x.SupplierId == id))
        //     //       .To<ArticleConformityExportModel>()
        //     //       .ToListAsync();
        //     // foreach (var article in model.Articles)
        //     // {
        //     //     article.ArticleConformity = this.conformitiesRepository
        //     //         .AllAsNoTracking()
        //     //         .Where(c => c.ArticleId == article.ArticleId &&
        //     //                     c.SupplierId == model.Id &&
        //     //                     c.ConformityTypeId == article.ConformityTypeId)
        //     //         .To<ConformityValidityExportModel>()
        //     //         .FirstOrDefault();
        //     // }

        // // var a = model.Articles.Where(x => x.ArticleConformity != null).ToList();
        //     return model;
        // }
        public async Task CreateAsync(SupplierCreateInputModel input, string userId)
        {
            // TODO - to give the numbers automatically
            var entity = new Supplier
            {
                Number = input.Number.Trim().ToUpper(),
                Name = input.Name.Trim().ToUpper(),
                Email = input.Email?.Trim(),
                PhoneNumber = input.PhoneNumber?.Trim(),
                ContactPersonFirstName = input.ContactPersonFirstName == null ? null :
                             PascalCaseConverter.Convert(input.ContactPersonFirstName),
                ContactPersonLastName = input.ContactPersonLastName == null ? null :
                             PascalCaseConverter.Convert(input.ContactPersonLastName),
                UserId = userId,
            };

            await this.suppliersRepository.AddAsync(entity);

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(SupplierEditInputModel input, string userId)
        {
            var entity = await this.suppliersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            entity.Name = input.Name?.Trim().ToUpper();
            entity.Email = input.Email?.Trim();
            entity.PhoneNumber = input.PhoneNumber?.Trim();
            entity.ContactPersonFirstName = input.ContactPersonFirstName == null ?
                null : PascalCaseConverter.Convert(input.ContactPersonFirstName);
            entity.ContactPersonLastName = input.ContactPersonLastName == null ?
                null : PascalCaseConverter.Convert(input.ContactPersonLastName);
            entity.UserId = userId;

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id, string userId)
        {
            var entity = await this.suppliersRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            this.suppliersRepository.Delete(entity);

            var articleSuppliersEntities = await this.articleSuppliersRepository
                                                        .All()
                                                        .Where(x => x.SupplierId == id)
                                                        .ToListAsync();

            foreach (var articleSuppliersEntity in articleSuppliersEntities)
            {
                this.articleSuppliersRepository.Delete(articleSuppliersEntity);
            }

            var supplierConformitiesEntities = await this.conformitiesRepository
                                                            .All()
                                                            .Where(x => x.SupplierId == id)
                                                            .ToListAsync();

            foreach (var supplierConformitiesEntity in supplierConformitiesEntities)
            {
                this.conformitiesRepository.Delete(supplierConformitiesEntity);
            }

            return await this.suppliersRepository.SaveChangesAsync();
        }
    }
}
