namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using ConformityCheck.Web.ViewModels.Products;
    using ConformityCheck.Web.ViewModels.Substances;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypesRepository;

        public ArticlesService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
            this.usersRepository = usersRepository;
            this.articleConformityTypesRepository = articleConformityTypeRepository;
        }

        public int GetCount()
        {
            return this.articlesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.articlesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.articlesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var articles = await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var entity = await this.articlesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            return entity;
        }

        public async Task<Article> GetByIdAsync(string id)
        {
            var entity = await this.articlesRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            return entity;
        }

        public async Task CreateAsync(ArticleCreateInputModel input)
        {
            var articleEntity = await this.articlesRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Number == input.Number.Trim().ToUpper());

            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var article = new Article
            {
                Number = input.Number.Trim().ToUpper(),
                Description = this.PascalCaseConverter(input.Description),

                //UserId = userEntity.Id,
            };

            await this.articlesRepository.AddAsync(article);

            await this.articlesRepository.SaveChangesAsync();

            if (input.SupplierId != null)
            {
                await this.AddSupplierAsync(new ArticleManageSuppliersInputModel()
                {
                    Id = article.Id,
                    SupplierId = input.SupplierId,
                });
            }

            if (input.ConformityTypes.Any())
            {
                await this.AddConformityTypesAsync(article, input.ConformityTypes);
            }

            // TODO: products, substances to be added too.
        }

        public async Task EditAsync(ArticleEditInputModel input)
        {
            var articleEntity = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            if (input.Description != null)
            {
                articleEntity.Description = this.PascalCaseConverter(input.Description);
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var entity = await this.articlesRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new ArgumentException("No such article id.");
            }

            this.articlesRepository.Delete(entity);

            // TODO - da razbera kak da naprawq triene, no da mi istanat zapisite. Sigurno trqbwa da
            // vkaram kolona IsDeleted vyv vsqka tablica ot dolnite 4...
            // article.IsDeleted = true;
            // foreach (var item in article.Suppliers)
            // {
            // }
            // article.Suppliers.Clear();
            // article.Substances.Clear();
            // article.Products.Clear();
            // article.Conformities.Clear();

            return await this.articlesRepository.SaveChangesAsync();
        }

        public async Task AddSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleSuppliers = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(a => a.ArticleId == input.Id)
                .ToListAsync();

            var initialNumberOfSuppliers = articleSuppliers.Count();

            if (articleSuppliers.Any(x => x.SupplierId == input.SupplierId))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            var supplierEntity = await this.suppliersRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == input.SupplierId);

            if (supplierEntity == null)
            {
                throw new ArgumentException("There is no such supplier created.");
            }

            var articleSupplierEntity = new ArticleSupplier
            {
                ArticleId = input.Id,
                SupplierId = input.SupplierId,
            };

            await this.articleSuppliersRepository.AddAsync(articleSupplierEntity);

            if (initialNumberOfSuppliers == 0)
            {
                articleSupplierEntity.IsMainSupplier = true;
            }

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task ChangeMainSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleEntity = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            var articleSupplierEntitis = await this.articleSuppliersRepository
                .All()
                .Where(x => x.ArticleId == input.Id)
                .ToListAsync();

            foreach (var entity in articleSupplierEntitis)
            {
                if (entity.SupplierId == input.SupplierId)
                {
                    entity.IsMainSupplier = true;
                }
                else
                {
                    entity.IsMainSupplier = false;
                }
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task RemoveSupplierAsync(ArticleManageSuppliersInputModel input)
        {
            var articleSupplierEntity = await this.articleSuppliersRepository.All()
                .FirstOrDefaultAsync(x => x.ArticleId == input.Id && x.SupplierId == input.SupplierId);

            if (articleSupplierEntity == null)
            {
                throw new ArgumentException("No such article or supplier.");
            }

            this.articleSuppliersRepository.Delete(articleSupplierEntity);

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task AddConformityTypeAsync(ArticleManageConformityTypesInputModel input)
        {
            await this.AddConformityTypesAsync(
                new Article { Id = input.Id },
                new int[] { input.ConformityTypeId });
        }

        public async Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes)
        {
            foreach (var conformityType in conformityTypes)
            {
                if (!this.conformityTypesRepository.All().Any(x => x.Id == conformityType))
                {
                    throw new ArgumentException($"There is no such conformity type.");
                }

                var articleConformityType = await this.articleConformityTypesRepository
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(x => x.ArticleId == article.Id && x.ConformityTypeId == conformityType);

                if (articleConformityType != null)
                {
                    continue;
                    //throw new ArgumentException($"This conformity type is already asigned to this article.");
                }

                await this.articleConformityTypesRepository.AddAsync(new ArticleConformityType
                {
                    ArticleId = article.Id,
                    ConformityTypeId = conformityType,
                });
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task RemoveConformityTypesAsync(ArticleManageConformityTypesInputModel input)
        {
            var articleConformityTypeEntity = await this.articleConformityTypesRepository
                .All()
                .FirstOrDefaultAsync(x => x.ArticleId == input.Id && x.ConformityTypeId == input.ConformityTypeId);

            if (articleConformityTypeEntity == null)
            {
                throw new ArgumentException("No article with this conformity type.");
            }

            this.articleConformityTypesRepository.Delete(articleConformityTypeEntity);

            await this.articleConformityTypesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetSuppliersByIdAsync<T>(string id)
        {
            return await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == id)
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<ConformityTypeConformitySupplierModel>> GetConformityTypesByIdAsync(string articleId, string supplierId)
        {
            var entities = await this.articleConformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .OrderBy(x => x.ConformityTypeId)
                .To<ConformityTypeConformitySupplierModel>()
                .ToListAsync();

            foreach (var entity in entities)
            {
                if (entity.SupplierId == supplierId
                    && entity.ConformityIsAccepted
                    && entity.ConformityIsValid)
                {
                    entity.SupplierConfirmed = true;
                }
            }

            return entities;
        }

        // not in the Interface!
        public IEnumerable<T> ListArticleConformities<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ListArticleSuppliers<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ListArticleProducts<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SearchByArticleNumber<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SearchByConformityType<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SearchByConfirmedStatus<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SearchBySupplierNumber<T>(string id)
        {
            throw new NotImplementedException();
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



        // for delete:
        //public IEnumerable<string> GetSuppliersIdsList(string articleId)
        //{
        //    return this.articlesRepository.All()
        //        .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers.Select(s => s.Supplier.Id)).FirstOrDefault();
        //}
        //public int GetSuppliersCount(string articleId)
        //{
        //    return this.articlesRepository.All()
        //        .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers).FirstOrDefault().Count;
        //}
        //public bool IsArticleFullyConfirmed(string articleId)
        //{
        //    return this.articlesRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == articleId)
        //    .ArticleConformityTypes.All(x => x.Conformity.IsAccepted);
        //}
        //public IEnumerable<string> GetSuppliersNumbersList(string articleId)
        //{
        //    return this.articlesRepository.All()
        //        .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers.Select(s => s.Supplier.Number)).FirstOrDefault();
        //}
        // private string FormatInputString(string stringToFormat) //it is 25% slower than the PascalCaseConverter
        // {
        //    return $"{stringToFormat.ToUpper()[0]}{stringToFormat.Substring(1).ToLower()}".Trim();
        // }
    }
}
