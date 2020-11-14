namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface IConformityTypesSeedService
    {
        Task CreateAsync(ConformityTypeDTO conformityTypeImputDTO);
    }
}
