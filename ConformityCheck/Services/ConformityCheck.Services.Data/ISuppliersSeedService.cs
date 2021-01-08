namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface ISuppliersSeedService
    {
        Task CreateAsync(SupplierImportDTO supplierInputModel);
    }
}
