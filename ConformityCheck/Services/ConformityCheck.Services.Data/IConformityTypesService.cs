namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService<int>
    {
        Task CreateAsync(ConformityTypeModel input);

        Task EditAsync(ConformityTypeModel input);

        Task<int> DeleteAsync(int id);
    }
}
