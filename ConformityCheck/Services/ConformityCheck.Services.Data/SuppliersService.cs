namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

        public SuppliersService(
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository)
        {
            this.suppliersRepository = suppliersRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
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

        public async Task CreateAsync(CreateSupplierInputModel supplierInputModel)
        {
            var supplierEntity = this.suppliersRepository.AllAsNoTracking()
                .Where(x => x.Name == supplierInputModel.Name.Trim().ToUpper() ||
                x.Number == supplierInputModel.Number.Trim().ToUpper()).FirstOrDefault();

            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            if (supplierEntity != null)
            {
                throw new ArgumentException($"There is already a supplier with this number or name.");
            }

            supplierEntity = new Supplier
            {
                Number = supplierInputModel.Number.Trim().ToUpper(),
                Name = this.PascalCaseConverter(supplierInputModel.Name),
                Email = supplierInputModel.Email?.Trim(),
                PhoneNumber = supplierInputModel.PhoneNumber?.Trim(),
                ContactPersonFirstName = supplierInputModel.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(supplierInputModel.ContactPersonFirstName),
                ContactPersonLastName = supplierInputModel.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(supplierInputModel.ContactPersonLastName),
            };

            await this.suppliersRepository.AddAsync(supplierEntity);

            await this.suppliersRepository.SaveChangesAsync();
        }

        //public async Task<Supplier> GetOrCreateSupplierAsync(CreateArticleInputModel articleViewModel)
        //{
        //    

        //    return supplierEntity;
        //}
        private string PascalCaseConverter(string stringToFix)
        {
            var st = new StringBuilder();
            st.Append(char.ToUpper(stringToFix[0]));
            for (int i = 1; i < stringToFix.Length; i++)
            {
                st.Append(char.ToLower(stringToFix[i]));
            }

            return st.ToString().Trim();
        }
    }
}
