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
            IRepository<ArticleSubstance> articleSubstancesRepository)
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
        }

        public bool ArticleEntityCheck(string id)
        {
            var a = this.articlesRepository.AllAsNoTracking().Any(x => x.Id == id);

            return a;
        }

        public bool ArticleSupplierEntityCheck(string articleId, string supplierId)
        {
            return this.articleSuppliersRepository
                .AllAsNoTracking()
                .Any(x => x.ArticleId == articleId &&
                            x.SupplierId == supplierId);
        }

        public bool ConformityTypeEntityCheck(int id)
        {
            return this.conformityTypesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool ConformityTypeEntityCheck(string id)
        {
            return this.conformitiesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool ProductEntityCheck(string id)
        {
            return this.productsRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool SubstanceEntityCheck(int id)
        {
            return this.substancesRepository.AllAsNoTracking().Any(x => x.Id == id);
        }

        public bool SupplierEntityCheck(string id)
        {
            return this.suppliersRepository.AllAsNoTracking().Any(x => x.Id == id);
        }
    }
}
