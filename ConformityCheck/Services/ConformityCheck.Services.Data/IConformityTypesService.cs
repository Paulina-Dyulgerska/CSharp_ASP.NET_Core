namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public interface IConformityTypesService : IService<int>
    {
        Task CreateAsync(ConformityTypeCreateInputModel input, string userId);

        Task EditAsync(ConformityTypeEditInputModel input, string userId);
    }
}
