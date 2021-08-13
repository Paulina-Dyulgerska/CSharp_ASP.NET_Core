namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ContactFormEntries;

    public interface IContactFormEntriesService : IService<int>
    {
        Task CreateAsync(ContactFormEntryViewModel input, string userId);
    }
}
