﻿namespace ConformityCheck.Services.Data
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

        public IEnumerable<T> GetAllAsNoTracking<T>()
        {
            return this.articlesRepository.AllAsNoTracking().To<T>().ToList();
        }


        public IEnumerable<ArticleExportModel> GetAllAsNoTrackingFullInfo()
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

            var articles = this.articlesRepository.AllAsNoTracking().To<ArticleExportModel>().ToList();

            return articles;
        }

        public async Task CreateAsync(CreateArticleInputModel articleInputModel)
        {
            var articleEntity = this.articlesRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == articleInputModel.Number.Trim().ToUpper());

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
                await this.AddSupplierToArticleAsync(article, articleInputModel.Supplier.Id);
            }

            if (articleInputModel.ConformityTypes.Any())
            {
                await this.AddConformityTypesToArticleAsync(article, articleInputModel.ConformityTypes);
            }

            // TODO: products, substances to be added too.
        }

        private async Task AddConformityTypesToArticleAsync(Article article, IEnumerable<int> conformityTypes)
        {
            foreach (var conformityType in conformityTypes)
            {
                article.ArticleConformityTypes.Add(new ArticleConformityType
                {
                    ArticleId = article.Id,
                    ConformityTypeId = conformityType,
                });
            }

            await this.articlesRepository.SaveChangesAsync();
        }

        private async Task AddSupplierToArticleAsync(Article article, string supplierId)
        {
            var articleSuppliers = this.articleSuppliersRepository.All()
                .Where(a => a.ArticleId == article.Id)
                .ToList();

            if (articleSuppliers.Any(x => x.SupplierId == supplierId))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            var supplierEntity = this.suppliersRepository.All().FirstOrDefault(x => x.Id == supplierId);

            if (supplierEntity == null)
            {
                throw new ArgumentException("No such supplier");
            }

            await this.articleSuppliersRepository.AddAsync(new ArticleSupplier
            {
                ArticleId = article.Id,
                SupplierId = supplierId,
                IsMainSupplier = true,
            });

            var a = await this.articleSuppliersRepository.SaveChangesAsync();
        }

        public void DeleteSupplierFromArticle(string articleId, string supplierId)
        {
            var articleEntity = this.GetArticle(articleId);
            var supplierEntity = this.GetSupplier(supplierId);

            if (articleEntity == null)
            {
                throw new ArgumentException("No such article");
            }

            if (supplierEntity == null)
            {
                throw new ArgumentException("No such supplier");
            }

            var articleSuppliers = this.GetSuppliersIdsList(articleId);

            if (!articleSuppliers.Contains(supplierId))
            {
                throw new ArgumentException("The article does not have such supplier.");
            }

            this.articleSuppliersRepository.Delete(new ArticleSupplier
            {
                ArticleId = articleId,
                SupplierId = supplierId,
            });

            this.articleSuppliersRepository.SaveChangesAsync();
        }

        public Task<int> DeleteArticleAsync(string articleId)
        {
            var articleEntity = this.GetArticle(articleId);

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

            // async-await
            return this.articlesRepository.SaveChangesAsync();
        }

        public void AddConformityToArticle(string articleId, string supplierId, ArticleConformityImportDTO articleConformityImportDTO)
        {
            var supplierEntity = this.GetSupplier(supplierId);
            var articleEntity = this.GetArticle(articleId);

            if (articleEntity == null)
            {
                throw new ArgumentException("No such article");
            }

            if (supplierEntity == null)
            {
                throw new ArgumentException("No such supplier");
            }

            var articleSuppliers = this.GetSuppliersIdsList(articleId);

            if (!articleSuppliers.Contains(supplierId))
            {
                throw new ArgumentException("The article does not have such supplier.");
            }

            // to create table in the dbContext ArticleConformitys as a dbContextSet, to make all dbContextSets ending s or without s!!! But similar!!
            // to chech dali veche nqmam takowa conformity na tozi article s tozi dostawchik!!!!
            {
            }

            var conformityType = this.conformityTypesRepository.All()
                .FirstOrDefault(ct => ct.Description == articleConformityImportDTO.ConformityType.Trim().ToUpper());

            if (conformityType == null)
            {
                throw new ArgumentException("No such conformity type.");
            }

            // if such conformity type already exist, what we are doing?
            {
            }

            var conformity = new Conformity
            {
                ConformityTypeId = conformityType.Id,
                IssueDate = articleConformityImportDTO.IssueDate,
                ConformationAcceptanceDate = articleConformityImportDTO.ConformationAcceptanceDate,
                IsAccepted = articleConformityImportDTO.IsAssepted,
                IsValid = true,
                SupplierId = supplierEntity.Id,
                Comments = articleConformityImportDTO.Comments,
            };

            this.conformitiesRepository.AddAsync(conformity);

            //articleEntity.ArticleConformities.Add(new ArticleConformity
            //{
            //    Article = articleEntity,
            //    Conformity = conformity,
            //});

            // TODO async-await
            this.conformitiesRepository.SaveChangesAsync();
        }

        // update conformity with new one????
        public void DeleteConformity(string articleId)
        {
            throw new NotImplementedException();
        }

        public EditExportModel GetEditArticle(string articleId)
        {
            var articleEntity = this.GetArticle(articleId);

            // TODO - async-await
            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            var articleExport = this.articlesRepository.All().To<ArticleExportModel>()
                .FirstOrDefault(x => x.Id == articleId);
            var article = new EditExportModel { Export = articleExport };
            return article;
        }

        public async Task PostEditArticleAsync(EditExportModel articleInputModel)
        {
            var articleEntity = this.articlesRepository.All().FirstOrDefault(x => x.Id ==
            articleInputModel.Export.Id);

            // TODO - async-await
            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            if (articleInputModel.Create.Description != null)
            {
                articleEntity.Description = this.PascalCaseConverter(articleInputModel.Create.Description);

            }

            if (articleInputModel.Create.Supplier.Id != null)
            {
                // TODO: to check the maintsupplier!!!
                await this.AddSupplierToArticleAsync(articleEntity, articleInputModel.Create.Supplier.Id);
            }

            // TODO: to check is this already there
            if (articleInputModel.Create.ConformityTypes != null)
            {
                await this.AddConformityTypesToArticleAsync(articleEntity, articleInputModel.Create.ConformityTypes);
            }

            // TODO - all other article characteristics have to be able to be updated from this method!!! S buttons +add +add na vseki
            // supplier, product, substance i t.n. A otstrani shte ima - za delete na vseki zapis!!!
            // Suppliers - AddSupplierToArticle, DeleteSupplierFromArticle
            // Conformities -AddConformity, DeleteConformity
            // Products - ListArticleProducts only, no delete
            // Sustances - Add/DeleteSubstance!!!! To have it in the interface
            {
            }

            // TODO - async-await
            //this.articlesRepository.SaveChangesAsync();
        }

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

        public Article GetArticle(string articleId)
        {
            return this.articlesRepository.All().FirstOrDefault(x => x.Id == articleId);
        }

        public Supplier GetSupplier(string supplierId)
        {
            return this.suppliersRepository.All().FirstOrDefault(x => x.Id == supplierId);
        }

        public IEnumerable<string> GetSuppliersNumbersList(string articleId)
        {
            return this.articlesRepository.All()
                .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers.Select(s => s.Supplier.Number)).FirstOrDefault();
        }

        public IEnumerable<string> GetSuppliersIdsList(string articleId)
        {
            return this.articlesRepository.All()
                .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers.Select(s => s.Supplier.Id)).FirstOrDefault();
        }

        public int GetSuppliersCount(string articleId)
        {
            return this.articlesRepository.All()
                .Where(x => x.Id == articleId).Select(x => x.ArticleSuppliers).FirstOrDefault().Count;
        }

        //public bool IsArticleFullyConfirmed(string articleId)
        //{
        //    return this.articlesRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == articleId)
        //    .ArticleConformityTypes.All(x => x.Conformity.IsAccepted);
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


        // private string FormatInputString(string stringToFormat) //it is 25% slower than the PascalCaseConverter
        // {
        //    return $"{stringToFormat.ToUpper()[0]}{stringToFormat.Substring(1).ToLower()}".Trim();
        // }
    }
}