namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public IEnumerable<T> GetAllAsNoTracking<T>()
        {
            return this.substancesRepository.AllAsNoTracking().To<T>().ToList();
        }

        public Task<T> GetByIdAsync<T>(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsPagesAsync<T>(int page, int itemsPerPage = 12)
        {
            throw new System.NotImplementedException();
        }
    }
}
