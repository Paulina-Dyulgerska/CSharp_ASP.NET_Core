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

    public class ArticleService : IArticleService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;

        public ArticleService(
            IDeletableEntityRepository<Article> articlesRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository,
            IRepository<ArticleSupplier> articleSuppliersRepository,
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.suppliersRepository = suppliersRepository;
            this.articleSuppliersRepository = articleSuppliersRepository;
            this.conformityTypesRepository = conformityTypesRepository;
            this.conformitiesRepository = conformitiesRepository;
        }

        public int GetCount()
        {
            return this.articlesRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.articlesRepository.All().To<T>().ToList();
        }

        public async Task CreateAsync(ArticleImportDTO articleImportDTO)
        {
            var articleEntity = this.articlesRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

            // TODO - async-await
            if (articleEntity != null)
            {
                throw new ArgumentException($"There is already an article with this number.");
            }

            var article = new Article
            {
                Number = articleImportDTO.Number.Trim().ToUpper(),
                Description = this.PascalCaseConverter(articleImportDTO.Description),
            };

            await this.articlesRepository.AddAsync(article);

            await this.articlesRepository.SaveChangesAsync();

            if (articleImportDTO.SupplierName != null && articleImportDTO.SupplierNumber != null)
            {
                await this.AddSupplierToArticleAsync(article, articleImportDTO);
            }
        }

        public async Task<int> AddSupplierToArticleAsync(Article article, ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = await this.GetOrCreateSupplierAsync(articleImportDTO);

            var articleSuppliers = this.articlesRepository.All()
                .Where(a => a.Id == article.Id).Select(a => a.ArticleSuppliers).FirstOrDefault();

            if (articleSuppliers.Any(x => x.SupplierId == supplierEntity.Id))
            {
                throw new ArgumentException("The supplier is already asigned to this article");
            }

            article.ArticleSuppliers.Add(new ArticleSupplier { Supplier = supplierEntity });

            return await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<Supplier> GetOrCreateSupplierAsync(ArticleImportDTO articleImportDTO)
        {
            var supplierEntity = this.suppliersRepository.All()
                .FirstOrDefault(x => x.Number == articleImportDTO.SupplierNumber.Trim().ToUpper());

            // new supplier is created if not exist in the dbContext:
            if (supplierEntity == null)
            {
                supplierEntity = new Supplier
                {
                    Number = articleImportDTO.SupplierNumber.Trim().ToUpper(),
                    Name = this.PascalCaseConverter(articleImportDTO.SupplierName),
                    Email = articleImportDTO.SupplierEmail?.Trim(),
                    PhoneNumber = articleImportDTO.SupplierPhoneNumber?.Trim(),
                    ContactPersonFirstName = articleImportDTO.ContactPersonFirstName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonFirstName),
                    ContactPersonLastName = articleImportDTO.ContactPersonLastName == null ? null :
                            this.PascalCaseConverter(articleImportDTO.ContactPersonLastName),
                };

                await this.suppliersRepository.AddAsync(supplierEntity);

                await this.suppliersRepository.SaveChangesAsync();
            }

            return supplierEntity;
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

            articleEntity.ArticleConformities.Add(new ArticleConformity
            {
                Article = articleEntity,
                Conformity = conformity,
            });

            // TODO async-await
            this.conformitiesRepository.SaveChangesAsync();
        }

        // update conformity with new one????
        public void DeleteConformity(string articleId)
        {
            throw new NotImplementedException();
        }

        public void UpdateArticle(ArticleImportDTO articleImportDTO)
        {
            var articleEntity = this.articlesRepository.All().FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

            // TODO - async-await
            if (articleEntity == null)
            {
                throw new ArgumentException($"There is no article with this number.");
            }

            articleEntity.Number = articleImportDTO.Number.Trim().ToUpper();

            articleEntity.Description = this.PascalCaseConverter(articleImportDTO.Description);

            // TODO - all other article characteristics have to be able to be updated from this method!!! S buttons +add +add na vseki
            // supplier, product, substance i t.n. A otstrani shte ima - za delete na vseki zapis!!!
            // Suppliers - AddSupplierToArticle, DeleteSupplierFromArticle
            // Conformities -AddConformity, DeleteConformity
            // Products - ListArticleProducts only, no delete
            // Sustances - Add/DeleteSubstance!!!! To have it in the interface
            {
            }

            // TODO - async-await
            this.articlesRepository.SaveChangesAsync();
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
