namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;

    public class ConformitiesService : IConformitiesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;
        private readonly IRepository<ArticleSupplier> articleSuppliersRepository;
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;

        public ConformitiesService(
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
            return this.conformitiesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformitiesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformitiesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var conformities = await this.conformitiesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return conformities;
        }

        public async Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage = 12)
        {
            var conformities = await this.GetAllAsNoTrackingOrderedAsync<T>();
            return conformities.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.conformitiesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<ConformityCreateInputModel> GetForCreateAsync(ConformityEditGetModel input)
        {
            var articleEntity = await this.articlesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.ArticleId)
                .FirstOrDefaultAsync();
            var conformityTypeEntity = await this.conformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.ConformityTypeId)
                .FirstOrDefaultAsync();
            var supplierEntity = await this.suppliersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == input.SupplierId)
                .FirstOrDefaultAsync();

            var entity = new ConformityCreateInputModel
            {
                ArticleId = articleEntity.Id,
                ArticleNumber = articleEntity.Number,
                ArticleDescription = articleEntity.Description,
                ConformityTypeId = conformityTypeEntity.Id,
                ConformityTypeDescription = conformityTypeEntity.Description,
                SupplierId = supplierEntity.Id,
                SupplierName = supplierEntity.Name,
                SupplierNumber = supplierEntity.Number,
            };
           
            return entity;
        }

        public async Task<T> GetForEditAsync<T>(ConformityEditGetModel input)
        {
            return await this.conformitiesRepository
                    .AllAsNoTracking()
                    .Where(x => x.Id == input.Id)
                    .To<T>()
                    .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ConformityCreateInputModel input, string userId, string conformityFilePath)
        {
            if (input.ValidForSingleArticle)
            {
                await this.AddConformityToAnArticleAsync(input, userId, conformityFilePath);
                return;
            }

            var articleSuppliersEntities = await this.articleSuppliersRepository
                    .All()
                    .Where(x => x.SupplierId == input.SupplierId)
                    .ToListAsync();

            // za view componenta e towa: da se vika tq w controllera i da se prenasochva kym specialno view
            // za editvane na conformity, a ne za createvane!!!!

            foreach (var articleSupplierEntity in articleSuppliersEntities)
            {
                input.ArticleId = articleSupplierEntity.ArticleId;
                await this.AddConformityToAnArticleAsync(input, userId, conformityFilePath);
            }
        }

        public async Task EditAsync(ConformityEditInputModel input, string userId, string conformityFilePath)
        {
            // Already has such conformity to make an Edit!!!! File cannot be edited! Only delete and add new if
            // the file has to be updated!
            var conformityEntity = await this.conformitiesRepository
                                                .All()
                                                .FirstOrDefaultAsync(x => x.Id == input.Id);

            conformityEntity.IssueDate = input.IssueDate.Date;
            conformityEntity.IsAccepted = input.IsAccepted;
            conformityEntity.AcceptanceDate = DateTime.UtcNow.Date;
            conformityEntity.ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null;
            conformityEntity.Comments = input.Comments;
            conformityEntity.UserId = userId;

            if (input.ValidityDate != null && input.IsAccepted)
            {
                conformityEntity.ValidityDate = input.ValidityDate;
            }

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await this.conformitiesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.conformitiesRepository.Delete(entity);
            // this.conformitiesRepository.HardDelete(entity); ???
            await this.conformitiesRepository.SaveChangesAsync();
        }

        private async Task AddConformityToAnArticleAsync(
            ConformityCreateInputModel input,
            string userId,
            string conformityFilePath)
        {
            var currentConformity = await this.conformitiesRepository.All()
                .Where(x => x.ArticleId == input.ArticleId && x.SupplierId == input.SupplierId && x.ConformityTypeId == input.ConformityTypeId)
                .FirstOrDefaultAsync();

            if (currentConformity != null)
            {
               await this.DeleteAsync(currentConformity.Id);
            }

            var articleConformityType = this.articleConformityTypeRepository
                                    .All()
                                    .Any(x => x.ConformityTypeId == input.ConformityTypeId
                                                    && x.ArticleId == input.ArticleId);

            // Add conformity type to article if not assigned:
            if (!articleConformityType)
            {
                await this.articleConformityTypeRepository.AddAsync(new ArticleConformityType
                {
                    ConformityTypeId = input.ConformityTypeId,
                    ArticleId = input.ArticleId,
                });
            }

            var conformityEntity = new Conformity
            {
                ArticleId = input.ArticleId,
                SupplierId = input.SupplierId,
                ConformityTypeId = input.ConformityTypeId,
                IssueDate = input.IssueDate.Date,
                IsAccepted = input.IsAccepted,
                AcceptanceDate = DateTime.UtcNow.Date,
                ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null,
                Comments = input.Comments,
                UserId = userId,
            };

            if (input.ValidityDate != null && input.IsAccepted)
            {
                conformityEntity.ValidityDate = input.ValidityDate;
            }

            await this.conformitiesRepository.AddAsync(conformityEntity);

            Directory.CreateDirectory($"{conformityFilePath}/conformities/");
            var extension = Path.GetExtension(input.InputFile.FileName).ToLower().TrimStart('.');
            conformityEntity.FileExtension = extension;
            var physicalPath = $"{conformityFilePath}/conformities/{conformityEntity.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await input.InputFile.CopyToAsync(fileStream);

            await this.conformitiesRepository.SaveChangesAsync();
        }
    }
}
