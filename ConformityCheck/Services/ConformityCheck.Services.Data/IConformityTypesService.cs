namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService
    {
        Task CreateAsync(ConformityTypeModel input);

        Task<int> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingFullInfoAsync<T>();
    }
}
