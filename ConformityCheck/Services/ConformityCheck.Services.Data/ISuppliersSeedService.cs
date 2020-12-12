namespace ConformityCheck.Services.Data
{
    using ConformityCheck.Services.Data.Models;
    using System.Threading.Tasks;

    public interface ISuppliersSeedService
    {
        Task CreateAsync(SupplierImportDTO supplierInputModel);
    }
}
