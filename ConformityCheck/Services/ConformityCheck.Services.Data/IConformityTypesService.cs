namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService<int>
    {
        Task CreateAsync(ConformityTypeCreateInputModel input);

        Task EditAsync(ConformityTypeEditInputModel input);

        Task<int> DeleteAsync(int id);
    }
}
