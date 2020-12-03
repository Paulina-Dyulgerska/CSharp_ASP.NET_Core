namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService<int>
    {
        Task CreateAsync(ConformityTypeInputModel input);

        Task EditAsync(ConformityTypeInputModel input);

        Task<int> DeleteAsync(int id);
    }
}
