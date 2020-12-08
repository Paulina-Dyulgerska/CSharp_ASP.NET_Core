namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Conformities;

    public interface IConformitiesService : IService<string>
    {
        Task CreateAsync(ConformityCreateInputModel input);

        Task EditAsync(ConformityEditModel input);

        Task DeleteAsync(string id);

        Task<ConformityEditModel> GetForEditAsync(ConformityEditGetModel input);
    }
}