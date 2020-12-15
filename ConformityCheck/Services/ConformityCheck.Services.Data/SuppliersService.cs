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
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
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
            return await this.suppliersRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        //public async Task<SupplierDetailsModel> DetailsByIdAsync(string id)
        //{
        //    // SELECT*
        //    //  FROM ArticleConformityTypes AS ACONT
        //    //  LEFT JOIN ArticleSuppliers AS ASUP ON ACONT.ArticleId = ASUP.ArticleId
        //    //  JOIN Suppliers AS SUP ON ASUP.SupplierId = SUP.Id
        //    //  LEFT JOIN Conformities AS CON ON ASUP.ArticleId = CON.ArticleId
        //    //  WHERE SUP.Id = id
        //    var model = await this.GetByIdAsync<SupplierDetailsModel>(id);
        //    model.Articles = await this.articleConformityTypesRepository
        //                  .AllAsNoTracking()
        //                  .Where(ac => ac.Article.ArticleSuppliers.Any(x => x.SupplierId == id))
        //                  .Select(ac => new ArticleConformityModel
        //                  {
        //                      ArticleConformityType = ac,
        //                      ArticleConformity = ac.Article.Conformities
        //                                                      .AsQueryable()
        //                                                      .Where(x => x.SupplierId == id
        //                                                      && x.ConformityTypeId == ac.ConformityTypeId)
        //                                                      .To<ConformityExportModel>()
        //                                                      .FirstOrDefault(),
        //                  })
        //                  .ToListAsync();
        //    var a = model.Articles.Where(x => x.ArticleConformity != null).ToList();
        //    return model;
        //}

        public async Task<SupplierDetailsModel> DetailsByIdAsync(string id)
        {
            // SELECT*
            //  FROM ArticleConformityTypes AS ACONT
            //  LEFT JOIN ArticleSuppliers AS ASUP ON ACONT.ArticleId = ASUP.ArticleId
            //  JOIN Suppliers AS SUP ON ASUP.SupplierId = SUP.Id
            //  LEFT JOIN Conformities AS CON ON ASUP.ArticleId = CON.ArticleId
            //  WHERE SUP.Id = id
            var model = await this.GetByIdAsync<SupplierDetailsModel>(id);
            model.Articles = await this.articleConformityTypesRepository
              .AllAsNoTracking()
              .Where(ac => ac.Article.ArticleSuppliers.Any(x => x.SupplierId == id))
              .Select(ac => new ArticleConformityModel
              {
                  ArticleId = ac.ArticleId,
                  ArticleNumber = ac.Article.Number,
                  ArticleDescription = ac.Article.Description,
                  ConformityTypeId = ac.ConformityTypeId,
                  ConformityTypeDescription = ac.ConformityType.Description,
                  ArticleConformity = ac.Article.Conformities
                                                  .Where(x => x.SupplierId == id
                                                  && x.ConformityTypeId == ac.ConformityTypeId)
                                                  .FirstOrDefault(),
              })
              .ToListAsync();

            var a = model.Articles.Where(x => x.ArticleConformity != null).ToList();

            return model;
        }

        public async Task CreateAsync(SupplierCreateInputModel input)
        {
            // TODO - to give the numbers authomaticaly!!!
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.
            var entity = new Supplier
            {
                Number = input.Number.Trim().ToUpper(),
                Name = input.Name.Trim().ToUpper(),
                Email = input.Email?.Trim(),
                PhoneNumber = input.PhoneNumber?.Trim(),
                ContactPersonFirstName = input.ContactPersonFirstName == null ? null :
                             this.PascalCaseConverterWords(input.ContactPersonFirstName),
                ContactPersonLastName = input.ContactPersonLastName == null ? null :
                             this.PascalCaseConverterWords(input.ContactPersonLastName),
            };

            await this.suppliersRepository.AddAsync(entity);

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(SupplierEditInputModel input)
        {
            var entity = await this.suppliersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.
            entity.Name = input.Name.Trim().ToUpper();
            entity.Email = input.Email?.Trim();
            entity.PhoneNumber = input.PhoneNumber?.Trim();
            entity.ContactPersonFirstName = input.ContactPersonFirstName == null ?
                null : this.PascalCaseConverterWords(input.ContactPersonFirstName);
            entity.ContactPersonLastName = input.ContactPersonLastName == null ?
                null : this.PascalCaseConverterWords(input.ContactPersonLastName);

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetArticlesByIdAsync<T>(string id)
        {
            var articles = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(x => x.SupplierId == id)
                .OrderBy(x => x.Article.Number)

                // .Select(x => new Article
                // {
                //    Id = x.ArticleId,
                //    Number = x.Article.Number,
                //    Description = x.Article.Description,
                // })
                .To<T>()
                .ToListAsync();

            return articles;
        }

        private string PascalCaseConverterWords(string stringToFix)
        {
            var st = new StringBuilder();
            var wordsInStringToFix = stringToFix.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsInStringToFix)
            {
                st.Append(char.ToUpper(word[0]));

                for (int i = 1; i < word.Length; i++)
                {
                    st.Append(char.ToLower(word[i]));
                }

                st.Append(' ');
            }

            return st.ToString().Trim();
        }

    }
}
