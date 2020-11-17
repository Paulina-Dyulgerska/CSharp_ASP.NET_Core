namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Web.ViewModels.Articles.ViewComponents;
    using ConformityCheck.Web.ViewModels.ConformityTypes.ViewComponents;
    using ConformityCheck.Web.ViewModels.Suppliers.ViewComponents;
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

        public async Task<IEnumerable<ArticleViewComponentModel>> GetAllArticlesAsync()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                .Select(x => new ArticleViewComponentModel
                {
                    Id = x.Id,
                    NumberAndDescription = $"{x.Number} - {x.Description}",
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticleViewComponentModel>> GetLastCreatedArticlesAsync()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.ModifiedOn)
                .ThenByDescending(x => x.CreatedOn)
                .Select(x => new ArticleViewComponentModel
                {
                    Id = x.Id,
                    NumberAndDescription = $"{x.Number} - {x.Description}",
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierViewComponentModel>> GetAllSuppliersAsync()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                .Select(x => new SupplierViewComponentModel
                {
                    Id = x.Id,
                    NameAndNumber = $"{x.Name} - {x.Number}",
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierViewComponentModel>> GetLastCreatedSuppliersAsync()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.ModifiedOn)
                .ThenByDescending(x => x.CreatedOn)
                .Select(x => new SupplierViewComponentModel
                {
                    Id = x.Id,
                    NameAndNumber = $"{x.Name} - {x.Number}",
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypeViewComponentModel>> GetAllConformityTypesAsync()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new ConformityTypeViewComponentModel
                {
                    Id = x.Id,
                    Description = x.Description,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypeViewComponentModel>> GetLastCreatedConformityTypesAsync()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.ModifiedOn)
                .ThenByDescending(x=>x.CreatedOn)
                .Select(x => new ConformityTypeViewComponentModel
                {
                    Id = x.Id,
                    Description = x.Description,
                })
                .ToListAsync();
        }

        //public async Task<<KeyValuePair<string, string>> GetAllProductsAsync()
        //{
        //    return await this.productsRepository
        //        .AllAsNoTracking()
        //        .OrderBy(x => x.Number)
        //        .Select(x => new
        //        {
        //            x.Id,
        //            x.Number,
        //        })
        //        .ToListAsync()
        //        .Select(x => new KeyValuePair<string, string>(x.Id, x.Number));
        //}

        //public async Task<<KeyValuePair<string, string>> GetLastCreatedProductsAsync()
        //{
        //    return await this.productsRepository
        //        .AllAsNoTracking()
        //        .OrderBy(x => x.Number)
        //        .Select(x => new
        //        {
        //            x.Id,
        //            x.Number,
        //        })
        //        .ToListAsync()
        //        .Select(x => new KeyValuePair<string, string>(x.Id, x.Number));
        //}

        //public async Task<<KeyValuePair<string, string>> GetAllSubstancesAsync()
        //{
        //    return await this.substancesRepository
        //        .AllAsNoTracking()
        //        .OrderBy(x => x.CASNumber)
        //        .Select(x => new
        //        {
        //            x.Id,
        //            x.CASNumber,
        //            x.Description,
        //        })
        //        .ToListAsync()
        //        .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), $"{x.CASNumber} - {x.Description}"));
        //}

        //public async Task<<KeyValuePair<string, string>> GetLastCreatedSubstancesAsync()
        //{
        //    return await this.substancesRepository
        //        .AllAsNoTracking()
        //        .OrderBy(x => x.CASNumber)
        //        .Select(x => new
        //        {
        //            x.Id,
        //            x.CASNumber,
        //            x.Description,
        //        })
        //        .ToListAsync()
        //        .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), $"{x.CASNumber} - {x.Description}"));
        //}

    }
}
