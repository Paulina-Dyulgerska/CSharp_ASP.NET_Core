namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
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
            if (searchInput is null)
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
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ArticleNumber)
                        .ThenBy(a => a.ConformityTypeDescription)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.DescriptionSortParam:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ArticleDescription)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.DescriptionSortParamDesc:
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ArticleDescription)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.ConformityTypeSortParam:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ConformityTypeDescription)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.ConformityTypeSortParamDesc:
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ConformityTypeDescription)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.HasConformitySortParam:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ArticleConformity?.Id)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.HasConformitySortParamDesc:
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ArticleConformity?.Id)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.AcceptedConformitySortParam:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ArticleConformity?.IsAccepted)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.AcceptedConformitySortParamDesc:
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ArticleConformity?.IsAccepted)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.ValidConformitySortParam:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ArticleConformity?.IsValid)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                case GlobalConstants.ValidConformitySortParamDesc:
                    supplier.Articles = supplier.Articles
                        .OrderByDescending(a => a.ArticleConformity?.IsValid)
                        .ThenBy(a => a.ArticleNumber)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;

                // case "number":
                default:
                    supplier.Articles = supplier.Articles
                        .OrderBy(a => a.ArticleNumber)
                        .ThenBy(a => a.ConformityTypeDescription)
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage);
                    return supplier;
            }
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
            // TODO - to give the numbers authomaticaly!!!
            var entity = new Supplier
            {
                Number = input.Number.Trim().ToUpper(),
                Name = input.Name.Trim().ToUpper(),
                Email = input.Email?.Trim(),
                PhoneNumber = input.PhoneNumber?.Trim(),
                ContactPersonFirstName = input.ContactPersonFirstName == null ? null :
                             this.PascalCaseConverterWords(input.ContactPersonFirstName),
                ContactPersonLastName = input.ContactPersonLastName == null ? null :
                             this.PascalCaseConverterWords(input.ContactPersonLastName),
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

            entity.Name = input.Name.Trim().ToUpper();
            entity.Email = input.Email?.Trim();
            entity.PhoneNumber = input.PhoneNumber?.Trim();
            entity.ContactPersonFirstName = input.ContactPersonFirstName == null ?
                null : this.PascalCaseConverterWords(input.ContactPersonFirstName);
            entity.ContactPersonLastName = input.ContactPersonLastName == null ?
                null : this.PascalCaseConverterWords(input.ContactPersonLastName);
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

        private string PascalCaseConverterWords(string stringToFix)
        {
            var st = new StringBuilder();
            var wordsInStringToFix = stringToFix.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsInStringToFix)
            {
                st.Append(char.ToUpper(word[0]));

                for (int i = 1; i < word.Length; i++)
                {
                    st.Append(char.ToLower(word[i]));
                }

                st.Append(' ');
            }

            return st.ToString().Trim();
        }
    }
}
