namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;

        public SuppliersService(IDeletableEntityRepository<Supplier> suppliersRepository)
        {
            this.suppliersRepository = suppliersRepository;
        }

        public int GetCount()
        {
            return this.suppliersRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.suppliersRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking<T>()
        {
            return this.suppliersRepository.AllAsNoTracking().To<T>().ToList();
        }

        //public async Task<Supplier> GetOrCreateSupplierAsync(CreateArticleInputModel articleViewModel)
        //{
        //    var supplierEntity = this.suppliersRepository.All()
        //        .FirstOrDefault(x => x.Id == articleViewModel.Supplier.Id);

        //    // new supplier is created if not exist in the dbContext:
        //    if (supplierEntity == null)
        //    {
        //        supplierEntity = new Supplier
        //        {
        //            Number = articleViewModel.Supplier.Number.Trim().ToUpper(),
        //            Name = this.PascalCaseConverter(articleViewModel.SupplierName),
        //            Email = articleViewModel.SupplierEmail?.Trim(),
        //            PhoneNumber = articleViewModel.SupplierPhoneNumber?.Trim(),
        //            ContactPersonFirstName = articleViewModel.ContactPersonFirstName == null ? null :
        //                    this.PascalCaseConverter(articleViewModel.ContactPersonFirstName),
        //            ContactPersonLastName = articleViewModel.ContactPersonLastName == null ? null :
        //                    this.PascalCaseConverter(articleViewModel.ContactPersonLastName),
        //        };

        //        await this.suppliersRepository.AddAsync(supplierEntity);

        //        await this.suppliersRepository.SaveChangesAsync();
        //    }

        //    return supplierEntity;
        //}
    }
}
