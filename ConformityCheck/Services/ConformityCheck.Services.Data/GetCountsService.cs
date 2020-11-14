namespace ConformityCheck.Services.Data
{
    using System.Linq;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;

    public class GetCountsService : IGetCountsService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Substance> substancesRepository;
        private readonly IDeletableEntityRepository<RegulationList> regulationListsRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public GetCountsService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Substance> substancesRepository,
            IDeletableEntityRepository<RegulationList> regulationListsRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.substancesRepository = substancesRepository;
            this.regulationListsRepository = regulationListsRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.productsRepository = productsRepository;
        }

        public CountsDto GetCounts()
        {
            return new CountsDto
            {
                Articles = this.articlesRepository.All().Count(),
                Suppliers = this.suppliersRepository.All().Count(),
                Products = this.productsRepository.All().Count(),
                Conformities = this.conformitiesRepository.All().Count(),
                ConformityTypes = this.conformityTypesRepository.All().Count(),
                Substances = this.substancesRepository.All().Count(),
                RegulationLists = this.regulationListsRepository.All().Count(),
            };
        }
    }
}
