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

        public int GetCountBySearchInput(string searchInput)
        {
            throw new System.NotImplementedException();
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

        public Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(string searchInput, string sortOrder, int page, int itemsPerPage)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(int id, string userId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: DeleteAsync - to check all tables with relations with Substance and is the raw in them needed or not
        // to be deleted too when I delete a Substance
    }
}
