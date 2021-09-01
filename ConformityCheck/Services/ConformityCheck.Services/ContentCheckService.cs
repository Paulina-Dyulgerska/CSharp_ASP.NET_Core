namespace ConformityCheck.Services
{
    using System.Linq;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;

    public class ContentCheckService : IContentCheckService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<Substance> substancesRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypesRepository;
        private readonly IRepository<ArticleProduct> articleProductsRepository;
        private readonly IRepository<ArticleSubstance> articleSubstancesRepository;
        private readonly IRepository<ProductConformityType> productConformityTypesRepository;

        public ContentCheckService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Substance> substancesRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IRepository<ArticleConformityType> articleConformityTypesRepository,
            IRepository<ArticleProduct> articleProductsRepository,
            IRepository<ArticleSubstance> articleSubstancesRepository,
            IRepository<ProductConformityType> productConformityTypesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.productsRepository = productsRepository;
            this.substancesRepository = substancesRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.articleConformityTypesRepository = articleConformityTypesRepository;
            this.articleProductsRepository = articleProductsRepository;
            this.articleSubstancesRepository = articleSubstancesRepository;
            this.productConformityTypesRepository = productConformityTypesRepository;
        }

        public bool ArticleEntityIdCheck(string id)
        {
            var a = this.articlesRepository.AllAsNoTracking().Any(x => x.Id == id);

            return a;
        }

        public bool ArticleSupplierEntityIdCheck(string articleId, string supplierId)
        {
            return this.articleSuppliersRepository
                .AllAsNoTracking()
                .Any(x => x.ArticleId == articleId &&
                            x.SupplierId == supplierId);
        }

        public bool ArticleConformityTypeEntityIdCheck(string articleId, int conformityTypeId)
        {
            return this.articleConformityTypesRepository
                .AllAsNoTracking()
                .Any(x => x.ArticleId == articleId &&
                            x.ConformityTypeId == conformityTypeId);
        }

        public bool ArticleEntityNumberCheck(string input)
        {
            return this.articlesRepository
                .AllAsNoTrackingWithDeleted()
                .Any(x => x.Number == input.Trim().ToUpper());
        }

        public bool ConformityTypeEntityIdCheck(int id)
        {
            return this.conformityTypesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool ConformityTypeEntityDescriptionCheck(string input)
        {
            return this.conformityTypesRepository
                .AllAsNoTracking()
                .Any(x => x.Description.ToUpper() == input.ToUpper());
        }

        public bool ConformityTypeArticlesCheck(int id)
        {
            return this.articleConformityTypesRepository
                .AllAsNoTracking()
                .Any(x => x.ConformityTypeId == id);
        }

        public bool ConformityTypeProductsCheck(int id)
        {
            return this.productConformityTypesRepository
                .AllAsNoTracking()
                .Any(x => x.ConformityTypeId == id);
        }

        public bool ConformityTypeConformitiesCheck(int id)
        {
            return this.conformitiesRepository
                .AllAsNoTracking()
                .Any(x => x.ConformityTypeId == id);
        }

        public bool ConformityEntityIdCheck(string id)
        {
            return this.conformitiesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool ProductEntityIdCheck(string id)
        {
            return this.productsRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool SubstanceEntityIdCheck(int id)
        {
            return this.substancesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool SupplierEntityIdCheck(string id)
        {
            return this.suppliersRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool SupplierEntityNameCheck(string input)
        {
            return this.suppliersRepository
                .AllAsNoTracking()
                .Any(x => x.Name == input.Trim().ToUpper());
        }

        public bool SupplierEntityNumberCheck(string input)
        {
            return this.suppliersRepository
                .AllAsNoTrackingWithDeleted()
                .Any(x => x.Number == input.Trim().ToUpper());
        }
    }
}
