namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Web.ViewModels.Conformities;

    public interface IConformitiesService : IService<string>
    {
        Task CreateAsync(ConformityCreateInputModel input, string userId, string conformityFilePath);

        Task EditAsync(ConformityEditInputModel input, string userId, string conformityFilePath);

        Task<ConformityCreateInputModel> GetForCreateAsync(ConformityGetInputModel input);

        Task AddRequestDateAsync(ConformityGetInputModel input, string userId);

        ConformityFileExportModel GetConformityFileFromLocalStorage(string conformityFileUrl);

        Task<ConformityFileExportModel> GetConformityFileFromBlobStorage(string conformityFileUrl);
    }
}
