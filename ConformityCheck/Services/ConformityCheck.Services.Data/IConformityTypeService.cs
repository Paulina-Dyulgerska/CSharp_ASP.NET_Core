namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface IConformityTypeService
    {
        void Create(ConformityTypeDTO conformityTypeImputDTO);

        IEnumerable<ConformityTypeDTO> ListAllConformityTypes();

        Task<int> Delete(int conformityTypeId);
    }
}
