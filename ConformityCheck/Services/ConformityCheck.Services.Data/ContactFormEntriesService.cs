namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Web.ViewModels.ContactFormEntries;

    public class ContactFormEntriesService : IContactFormEntriesService
    {
        private readonly IRepository<ContactFormEntry> contactFormEntriesRepository;

        public ContactFormEntriesService(IRepository<ContactFormEntry> contactFormEntriesRepository)
        {
            this.contactFormEntriesRepository = contactFormEntriesRepository;
        }

        public async Task CreateAsync(ContactFormEntryViewModel input, string userId)
        {
            var contactFormEntry = new ContactFormEntry
            {
                Name = input.Name,
                Email = input.Email,
                Title = input.Title,
                Content = input.Content,
                Ip = input.Ip,
                UserId = userId,
            };

            await this.contactFormEntriesRepository.AddAsync(contactFormEntry);

            await this.contactFormEntriesRepository.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(string searchInput, string sortOrder, int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            throw new NotImplementedException();
        }
    }
}
