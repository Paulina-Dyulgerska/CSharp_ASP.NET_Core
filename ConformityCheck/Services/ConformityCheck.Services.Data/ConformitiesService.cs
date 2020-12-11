﻿namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
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

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var entity = await this.conformitiesRepository
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

        public async Task CreateAsync(ConformityCreateInputModel input)
        {
            if (input.ValidForSingleArticle)
            {
                await this.AddConformityToAnArticle(input);

                await this.conformitiesRepository.SaveChangesAsync();

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
                await this.AddConformityToAnArticle(input);
            }

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public async Task<ConformityEditModel> GetForEditAsync(ConformityEditGetModel input)
        {
            var entity = await this.conformitiesRepository
                                .AllAsNoTracking()
                                .Where(x => x.ArticleId == input.ArticleId
                                   && x.SupplierId == input.SupplierId
                                   && x.ConformityTypeId == input.ConformityTypeId)
                                .To<ConformityEditModel>()
                                .FirstOrDefaultAsync();

            if (entity == null)
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

                entity = new ConformityEditModel
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
            }

            return entity;
        }

        public async Task EditAsync(ConformityEditModel input)
        {

        }


        private async Task AddConformityToAnArticle(ConformityCreateInputModel input)
        {
            //ArticleConformityType sa samo zadyljitelnite da gi ima potvyrdeni!!!
            // da rovq v Conformity i da sotavq tablicata ArticleConformityTypes samo s iskanite za artikula types, no 
            // da ne pazi i conformities vytre, a prez conformities da stawa vryzkata i s article i sys supplier i s
            //conformity type!!!!!! Da prerabotq DBmodels!!!! i wsichko tuk!!!

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

            // Already has such conformity?:
            var conformityEntity = await this.conformitiesRepository
                        .All()
                        .FirstOrDefaultAsync(x => x.ConformityTypeId == input.ConformityTypeId
                                        && x.ArticleId == input.ArticleId
                                        && x.SupplierId == input.SupplierId);

            if (conformityEntity != null)
            {
                this.conformitiesRepository.Delete(conformityEntity);
            }

            conformityEntity = new Conformity
            {
                ArticleId = input.ArticleId,
                SupplierId = input.SupplierId,
                ConformityTypeId = input.ConformityTypeId,
                IssueDate = input.IssueDate.Date,
                IsAccepted = input.IsAccepted,
                AcceptanceDate = DateTime.UtcNow.Date,
                ValidityDate = input.IsAccepted ? DateTime.UtcNow.Date.AddYears(3) : (DateTime?)null,
                Comments = input.Comments,
                FileUrl = "Az ne sym go naprawila oshte",
            };

            if (input.ValidityDate != null && input.IsAccepted)
            {
                conformityEntity.ValidityDate = input.ValidityDate;
            }

            await this.conformitiesRepository.AddAsync(conformityEntity);

            await this.conformitiesRepository.SaveChangesAsync();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        //Task AddConformityAsync(ArticleManageConformitiesModel input);
    }
}