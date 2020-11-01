using ConformityCheck.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConformityCheck.Services
{
    public interface IConformityTypeService
    {
        void Create(ConformityTypeDTO conformityTypeImputDTO);

        IEnumerable<ConformityTypeDTO> ListAllConformityTypes();

        Task<int> Delete(int conformityTypeId);
    }
}
