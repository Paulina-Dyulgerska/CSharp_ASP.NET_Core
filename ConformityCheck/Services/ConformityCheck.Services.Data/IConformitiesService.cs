namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Conformities;

    public interface IConformitiesService : IService<string>
    {
        Task CreateAsync(ConformityCreateInputModel input);

        Task EditAsync(ConformityEditInputModel input);

        Task DeleteAsync(string id);

        Task<ConformityEditInputModel> GetForEditAsync(ConformityEditGetModel input);
    }
}