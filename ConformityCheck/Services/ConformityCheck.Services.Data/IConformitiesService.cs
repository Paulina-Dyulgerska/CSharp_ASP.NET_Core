namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Conformities;

    public interface IConformitiesService : IService<string>
    {
        Task CreateAsync(ConformityCreateInputModel input, string userId, string conformityFilePath);

        Task EditAsync(ConformityEditInputModel input, string userId, string conformityFilePath);

        Task DeleteAsync(string id, string userId);

        Task<ConformityCreateInputModel> GetForCreateAsync(ConformityEditGetModel input);

        Task<T> GetForEditAsync<T>(ConformityEditGetModel input);
    }
}