namespace ConformityCheck.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.EntityFrameworkCore;

    public class ConformityTypesService : IConformityTypesService
    {
        private readonly IDeletableEntityRepository<ConformityType> conformityTypesRepository;
        private readonly IRepository<ArticleConformityType> articleConformityTypeRepository;
        private readonly IDeletableEntityRepository<Conformity> conformitiesRepository;

        public ConformityTypesService(
            IDeletableEntityRepository<ConformityType> conformityTypesRepository,
            IRepository<ArticleConformityType> articleConformityTypeRepository,
            IDeletableEntityRepository<Conformity> conformitiesRepository)
        {
            this.conformityTypesRepository = conformityTypesRepository;
            this.articleConformityTypeRepository = articleConformityTypeRepository;
            this.conformitiesRepository = conformitiesRepository;
        }

        public int GetCount()
        {
            return this.conformityTypesRepository.AllAsNoTracking().Count();
        }

        public int GetCountBySearchInput(string searchInput)
        {
            if (searchInput is null)
            {
                return this.GetCount();
            }

            return this.conformityTypesRepository
                .AllAsNoTracking()
                .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                           || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.conformityTypesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.conformityTypesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.conformityTypesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            var conformityTypes = await this.conformityTypesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Description)
                .ThenByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.ModifiedOn)
                .To<T>()
                .ToListAsync();

            return conformityTypes;
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(
            string sortOrder,
            int page,
            int itemsPerPage = 12)
        {
            // var conformityTypes = await this.GetAllAsNoTrackingOrderedAsync<T>();
            // return conformityTypes.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            switch (sortOrder)
            {
                case "id":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "idDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "description":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "descriptionDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "userEmail":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "userEmailDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "modifiedOn":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "modifiedOnDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                case "createdOn":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputAsync<T>(string searchInput)
        {
            var entities = await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .To<T>()
                                        .ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<T>> GetAllBySearchInputOrderedAsPagesAsync<T>(
            string searchInput,
            string sortOrder,
            int page,
            int itemsPerPage)
        {
            switch (sortOrder)
            {
                case "id":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "idDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Id)
                                        .ThenByDescending(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "description":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "descriptionDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.Description)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "userEmail":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "userEmailDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.User.Email)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "modifiedOn":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
                case "modifiedOnDesc":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .ThenBy(x => x.Id)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                case "createdOn":
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .OrderBy(x => x.CreatedOn)
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();

                // case "createdOnDesc": show last created first
                default:
                    return await this.conformityTypesRepository
                                        .AllAsNoTracking()
                                        .OrderByDescending(x => x.CreatedOn)
                                        .Where(x => x.Id.ToString().Contains(searchInput.ToUpper())
                                                    || x.Description.ToUpper().Contains(searchInput.ToUpper()))
                                        .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                        .To<T>()
                                        .ToListAsync();
            }
        }

        public async Task CreateAsync(ConformityTypeCreateInputModel input, string userId)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.
            var conformityType = new ConformityType
            {
                //Description = WebUtility.HtmlEncode(input.Description.ToUpper()),
                Description = input.Description.ToUpper(),
                UserId = userId,
            };

            await this.conformityTypesRepository.AddAsync(conformityType);

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ConformityTypeEditInputModel input, string userId)
        {
            // if this conformity type is not in the DB
            var entity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == input.Id);

            entity.Description = input.Description.ToUpper();
            entity.UserId = userId;

            await this.conformityTypesRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var entity = await this.conformityTypesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);
            entity.UserId = userId;

            // if this conformity type is assigned to article or product in the DB, no delete is possible - the
            // validation attribute ConformityTypeUsageAttribute is checking this.
            // if (this.conformitiesRepository.All().Any(ac => ac.ConformityTypeId == id))
            // {
            //    throw new ArgumentException($"Cannot delete conformity type with articles assigned to it.");
            // }
            this.conformityTypesRepository.Delete(entity);

            return await this.conformityTypesRepository.SaveChangesAsync();
        }
    }
}
