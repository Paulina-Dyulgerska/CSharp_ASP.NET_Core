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
    using ConformityCheck.Web.ViewModels.Suppliers.ViewComponents;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypesRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;

        public SuppliersService(
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository,
            IRepository<ArticleSupplier> articleSupplierRepository)
        {
            this.suppliersRepository = suppliersRepository;
            this.articleConformityTypesRepository = articleConformityTypeRepository;
            this.articleSuppliersRepository = articleSupplierRepository;
        }

        public int GetCount()
        {
            return this.suppliersRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.suppliersRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.suppliersRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.suppliersRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Number)
                .ThenByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var entity = await this.suppliersRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new ArgumentException($"There is no supplier with this number.");
            }

            return entity;
        }

        public async Task CreateAsync(CreateSupplierInputModel input)
        {
            var entity = await this.suppliersRepository.AllAsNoTracking()
                .Where(x => x.Name == input.Name.Trim().ToUpper() || x.Number == input.Number.Trim().ToUpper())
                .FirstOrDefaultAsync();

            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            if (entity != null)
            {
                throw new ArgumentException($"There is already a supplier with this number or name.");
            }

            entity = new Supplier
            {
                Number = input.Number.Trim().ToUpper(),
                Name = this.PascalCaseConverter(input.Name),
                Email = input.Email?.Trim(),
                PhoneNumber = input.PhoneNumber?.Trim(),
                ContactPersonFirstName = input.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(input.ContactPersonFirstName),
                ContactPersonLastName = input.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(input.ContactPersonLastName),
            };

            await this.suppliersRepository.AddAsync(entity);

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(SupplierEditInputModel input)
        {
            var entity = await this.suppliersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            if (entity == null)
            {
                throw new ArgumentException($"There is no such supplier.");
            }

            var hasThisName = await this.suppliersRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == input.Name.Trim().ToUpper() && x.Id != input.Id) != null;

            if (hasThisName)
            {
                throw new ArgumentException($"There is already a supplier with this name.");
            }

            entity.Name = this.PascalCaseConverter(input.Name);
            entity.Email = input.Email?.Trim();
            entity.PhoneNumber = input.PhoneNumber?.Trim();
            entity.ContactPersonFirstName = input.ContactPersonFirstName == null ?
                null : this.PascalCaseConverter(input.ContactPersonFirstName);
            entity.ContactPersonLastName = input.ContactPersonLastName == null ?
                null : this.PascalCaseConverter(input.ContactPersonLastName);

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetArticlesByIdAsync<T>(string id)
        {
            var articles = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(x => x.SupplierId == id)
                .OrderBy(x => x.Article.Number)
                //.Select(x => new Article
                //{
                //    Id = x.ArticleId,
                //    Number = x.Article.Number,
                //    Description = x.Article.Description,
                //})
                .To<T>()
                .ToListAsync();

            return articles;
        }

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
