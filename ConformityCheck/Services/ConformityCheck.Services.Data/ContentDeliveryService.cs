namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles.ViewComponents;
    using ConformityCheck.Web.ViewModels.ConformityTypes.ViewComponents;
    using ConformityCheck.Web.ViewModels.Products.ViewComponents;
    using ConformityCheck.Web.ViewModels.Substances.ViewComponents;
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

        public async Task<IEnumerable<ArticlesViewComponentModel>> GetAllArticlesAsync()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                //.Select(x => new ArticleViewComponentModel
                //{
                //    Id = x.Id,
                //    NumberAndDescription = $"{x.Number} - {x.Description}",
                //})
                .To<ArticlesViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticlesViewComponentModel>> GetLastCreatedArticlesAsync()
        {
            return await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                //.Select(x => new ArticleViewComponentModel
                //{
                //    Id = x.Id,
                //    NumberAndDescription = $"{x.Number} - {x.Description}",
                //})
                .To<ArticlesViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<SuppliersViewComponentModel>> GetAllSuppliersAsync()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Name)
                //.Select(x => new SupplierViewComponentModel
                //{
                //    Id = x.Id,
                //    NameAndNumber = $"{x.Name} - {x.Number}",
                //})
                .To<SuppliersViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<SuppliersViewComponentModel>> GetLastCreatedSuppliersAsync()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Name)
                //.Select(x => new SupplierViewComponentModel
                //{
                //    Id = x.Id,
                //    NameAndNumber = $"{x.Name} - {x.Number}",
                //})
                .To<SuppliersViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypesViewComponentModel>> GetAllConformityTypesAsync()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Description)
                //.Select(x => new ConformityTypeViewComponentModel
                //{
                //    Id = x.Id,
                //    Description = x.Description,
                //})
                .To<ConformityTypesViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypesViewComponentModel>> GetLastCreatedConformityTypesAsync()
        {
            return await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Description)
                //.Select(x => new ConformityTypeViewComponentModel
                //{
                //    Id = x.Id,
                //    Description = x.Description,
                //})
                .To<ConformityTypesViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductsViewComponentModel>> GetAllProductsAsync()
        {
            return await this.productsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Number)
                .To<ProductsViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductsViewComponentModel>> GetLstCreatedProductsAsync()
        {
            return await this.productsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<ProductsViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<SubstancesViewComponentModel>> GetAllSubstancesAsync()
        {
            return await this.substancesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.CASNumber)
                .To<SubstancesViewComponentModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<SubstancesViewComponentModel>> GetLastCreatedSubstancesAsync()
        {
            return await this.substancesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.CASNumber)
                .To<SubstancesViewComponentModel>()
                .ToListAsync();
        }
    }
}
