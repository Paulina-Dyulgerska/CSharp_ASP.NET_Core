namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface IConformityTypeService
    {
        Task CreateAsync(ConformityTypeDTO conformityTypeImputDTO);

        Task<int> DeleteAsync(int conformityTypeId);

        IEnumerable<ConformityTypeDTO> ListAllConformityTypes();
    }
}
