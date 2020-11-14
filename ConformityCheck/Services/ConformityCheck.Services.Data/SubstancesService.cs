namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SubstancesService : ISubstancesService
    {
        private readonly IDeletableEntityRepository<Substance> substancesRepository;

        public SubstancesService(IDeletableEntityRepository<Substance> substancesRepository)
        {
            this.substancesRepository = substancesRepository;
        }

        public int GetCount()
        {
            return this.substancesRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.substancesRepository.All().To<T>().ToList();
        }
    }
}
