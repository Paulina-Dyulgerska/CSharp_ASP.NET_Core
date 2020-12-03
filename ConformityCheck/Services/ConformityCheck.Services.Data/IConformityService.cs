using ConformityCheck.Web.ViewModels.Conformities;
using System.Threading.Tasks;

namespace ConformityCheck.Services.Data
{
    public interface IConformityService : IService<string>
    {
        Task CreateAsync(ConformityCreateInputModel input);
    }
}