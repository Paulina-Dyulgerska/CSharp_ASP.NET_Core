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
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Products;
    using ConformityCheck.Web.ViewModels.Substances;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

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
            this.articleConformityTypeRepository = articleConformityTypeRepository;
        }

        public int GetCount()
        {
            return this.articlesRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.articlesRepository.All().To<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.articlesRepository.All().To<T>().ToListAsync();
        }

        public IEnumerable<T> GetAllAsNoTracking<T>()
        {
            return this.articlesRepository.AllAsNoTracking().To<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.articlesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingFullInfoAsync<T>()
        {
            //var articles = this.articlesRepository.AllAsNoTracking().Select(a => new ArticleExportModel
            //{
            //    Id = a.Id,
            //    Number = a.Number,
            //    Description = a.Description,
            //    IsConfirmed = a.ArticleConformityTypes.All(x => x.Conformity != null && x.Conformity.IsAccepted),
            //    MainSupplierId = a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).SupplierId,
            //    MainSupplierName = a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Name,
            //    MainSupplierNumber = a.ArticleSuppliers.FirstOrDefault(x => x.IsMainSupplier).Supplier.Number,
            //    ArticleMissingConformityTypes = a.ArticleConformityTypes
            //    .Select(x => $"{x.ConformityType.Description} => {x.Conformity != null}")
            //    .ToList(),
            //    ArticleConformityTypes = a.ArticleConformityTypes
            //    .Select(x => $"{x.ConformityType.Description} => {x.Conformity.IsAccepted}")
            //    .ToList(),
            //}).ToList();

            var articles = await this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .ThenBy(x => x.Number)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task CreateAsync(CreateArticleInputModel articleInputModel)
        {
            var articleEntity = await this.articlesRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Number == articleInputModel.Number.Trim().ToUpper());

            //var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //take the user and record its id in the article, product, conformity, etc.

            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var article = new Article
            {
                Number = articleInputModel.Number.Trim().ToUpper(),
                Description = this.PascalCaseConverter(articleInputModel.Description),

                //UserId = userEntity.Id,
            };

            await this.articlesRepository.AddAsync(article);

            await this.articlesRepository.SaveChangesAsync();

            if (articleInputModel.Supplier.Id != null)
            {
                await this.AddSupplierAsync(article, articleInputModel.Supplier.Id, articleInputModel.Supplier.Id);
            }

            if (articleInputModel.ConformityTypes.Any())
            {
                await this.AddConformityTypesAsync(article, articleInputModel.ConformityTypes);
            }

            // TODO: products, substances to be added too.
        }

        public async Task AddConformityTypesAsync(Article article, IEnumerable<int> conformityTypes)
        {
            foreach (var conformityType in conformityTypes)
            {
                if (!this.conformityTypesRepository.AllAsNoTracking().Any(x => x.Id == conformityType))
                {
                    throw new ArgumentException($"There is no such conformity type.");
                }

                var articleConformityType = await this.articleConformityTypeRepository
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(x => x.ArticleId == article.Id && x.ConformityTypeId == conformityType);

                if (articleConformityType != null)
                {
                    continue;
                    //throw new ArgumentException($"This conformity is already asigned to this article.");
                }

                await this.articleConformityTypeRepository.AddAsync(new ArticleConformityType
                {
                    ArticleId = article.Id,
                    ConformityTypeId = conformityType,
                });
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task AddSupplierAsync(Article article, string supplierId, string mainSupplierId = null)
        {
            var articleSuppliers = await this.articleSuppliersRepository
                .AllAsNoTracking()
                .Where(a => a.ArticleId == article.Id)
                .ToListAsync();

            //taka maj e po-dobre vmesto gornoto
            //var articleSuppliers = article.ArticleSuppliers.ToList();

            if (articleSuppliers.Any(x => x.SupplierId == supplierId))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            var supplierEntity = await this.suppliersRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == supplierId);

            if (supplierEntity == null)
            {
                throw new ArgumentException("There is no such supplier created.");
            }

            var articleSupplierEntity = new ArticleSupplier
            {
                ArticleId = article.Id,
                SupplierId = supplierId,
            };

            await this.articleSuppliersRepository.AddAsync(articleSupplierEntity);

            if (supplierId == mainSupplierId)
            {
                articleSupplierEntity.IsMainSupplier = true;
            }
            else
            {
                var articleMainSupplierEntity = await this.articleSuppliersRepository.All().FirstOrDefaultAsync(x => x.ArticleId == article.Id
 && x.SupplierId == mainSupplierId);
                if (articleMainSupplierEntity != null)
                {
                    articleMainSupplierEntity.IsMainSupplier = true;
                }
            }

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task RemoveSupplierAsync(string articleId, string supplierId)
        {
            var articleEntity = await this.articlesRepository.All().FirstOrDefaultAsync(x => x.Id == articleId);
            var supplierEntity = await this.suppliersRepository.All().FirstOrDefaultAsync(x => x.Id == supplierId);

            if (articleEntity == null)
            {
                throw new ArgumentException("No such article");
            }

            if (supplierEntity == null)
            {
                throw new ArgumentException("No such supplier");
            }

            var hasThisSUpplier = articleEntity.ArticleSuppliers.Select(x => x.SupplierId)
                .Any(x => x == supplierId);

            if (!hasThisSUpplier)
            {
                throw new ArgumentException("The article does not have such supplier.");
            }

            this.articleSuppliersRepository.Delete(new ArticleSupplier
            {
                ArticleId = articleId,
                SupplierId = supplierId,
            });

            await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string articleId)
        {
            var articleEntity = await this.articlesRepository.All().FirstOrDefaultAsync(x => x.Id == articleId);

            if (articleEntity == null)
            {
                throw new ArgumentException("No such article id");
            }

            this.articlesRepository.Delete(articleEntity);

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
            {
            }

            return await this.articlesRepository.SaveChangesAsync();
        }

        public async Task AddConformityAsync(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO)
        {
            var articleEntity = await this.articlesRepository.All().FirstOrDefaultAsync(x => x.Id == articleId);
            var supplierEntity = await this.suppliersRepository.All().FirstOrDefaultAsync(x => x.Id == supplierId);

            if (articleEntity == null)
            {
                throw new ArgumentException("No such article");
            }

            if (supplierEntity == null)
            {
                throw new ArgumentException("No such supplier");
            }

            var hasThisSUpplier = articleEntity.ArticleSuppliers.Select(x => x.SupplierId)
                .Any(x => x == supplierId);

            if (!hasThisSUpplier)
            {
                throw new ArgumentException("The article does not have such supplier.");
            }

            // to create table in the dbContext ArticleConformitys as a dbContextSet, 
            //to make all dbContextSets ending s or without s!!! But similar!!
            // to chech dali veche nqmam takowa conformity na tozi article s tozi dostawchik!!!!
            {
            }

            var conformityType = await this.conformityTypesRepository.AllAsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == int.Parse(articleConformityImportDTO.ConformityType));

            if (conformityType == null)
            {
                throw new ArgumentException($"There is no such conformity type.");
            }

            if (!articleEntity.ArticleConformityTypes.Any(x => x.ConformityTypeId ==
                        int.Parse(articleConformityImportDTO.ConformityType)))
            {
                return;
                //throw new ArgumentException($"This conformity is already asigned to this article.");
            }

            //var hasConformity = articleEntity.ArticleConformityTypes
            //    .FirstOrDefault(x => x.ConformityTypeId == conformityType.Id).Conformity;
            var articleConformityType = await this.articleConformityTypeRepository.All()
                .FirstOrDefaultAsync(x => x.ConformityTypeId == conformityType.Id && x.ArticleId == articleId);

            if (articleConformityType.Conformity != null)
            {
                this.conformitiesRepository.Delete(articleConformityType.Conformity);
            }

            articleConformityType.Conformity = new Conformity
            {
                ConformityTypeId = conformityType.Id,
                IssueDate = articleConformityImportDTO.IssueDate,
                ConformationAcceptanceDate = articleConformityImportDTO.ConformationAcceptanceDate,
                IsAccepted = articleConformityImportDTO.IsAssepted,
                IsValid = true,
                SupplierId = supplierEntity.Id,
                Comments = articleConformityImportDTO.Comments,
                FileUrl = "Az ne sym go naprawila oshte",
            };

            await this.conformitiesRepository.AddAsync(articleConformityType.Conformity);

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public async Task DeleteConformityAsync(string articleId)
        {
            throw new NotImplementedException();
        }

        public async Task<EditExportModel> GetEditAsync(string articleId)
        {
            var articleEntity = await this.articlesRepository
                .All()
                .To<EditExportModel>()
                .FirstOrDefaultAsync(x => x.Id == articleId);

            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            return articleEntity;
        }

        public async Task PostEditAsync(EditExportModel articleInputModel)
        {
            var articleEntity = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == articleInputModel.Id);

            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            if (articleInputModel.Description != null)
            {
                articleEntity.Description = this.PascalCaseConverter(articleInputModel.Description);
            }

            if (articleInputModel.Supplier.Id != null)
            {
                // TODO: to check the maintsupplier!!!
                await this.AddSupplierAsync(articleEntity, articleInputModel.Supplier.Id, articleInputModel.MainSupplierId);
            }

            // TODO: to check is this already there
            if (articleInputModel.ConformityTypes != null)
            {
                await this.AddConformityTypesAsync(articleEntity, articleInputModel.ConformityTypes);
            }

            // TODO - all other article characteristics have to be able to be updated from this method!!! 
            //S buttons +add +add na vseki
            // supplier, product, substance i t.n. A otstrani shte ima - za delete na vseki zapis!!!
            // Suppliers - AddSupplierToArticle, DeleteSupplierFromArticle
            // Conformities -AddConformity, DeleteConformity
            // Products - ListArticleProducts only, no delete
            // Sustances - Add/DeleteSubstance!!!! To have it in the interface
            {
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        // not in the Interface!
        public IEnumerable<ConformityImportDTO> ListArticleConformities(string articleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SupplierExportDTO> ListArticleSuppliers(string articleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDTO> ListArticleProducts(string articleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleExportDTO> SearchByArticleNumber(string artileId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleExportDTO> SearchByConformityType(string conformityType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleExportDTO> SearchByConfirmedStatus(string status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleExportDTO> SearchBySupplierNumber(string supplierNumber)
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
