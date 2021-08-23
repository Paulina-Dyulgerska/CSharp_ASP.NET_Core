using ConformityCheck.Common;
using ConformityCheck.Data;
using ConformityCheck.Data.Common.Repositories;
using ConformityCheck.Data.Models;
using ConformityCheck.Data.Repositories;
using ConformityCheck.Services.Data.Models;
using ConformityCheck.Services.Mapping;
using ConformityCheck.Web.ViewModels;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConformityCheck.Services.Data.Tests
{

    //public class FakeUserManager : UserManager<ApplicationUser>
    //{
    //    public FakeUserManager() : base(
    //     new Mock<IUserStore<User>>().Object,
    //     new Mock<IOptions<IdentityOptions>>().Object,
    //     new Mock<IPasswordHasher<User>>().Object,
    //     new IUserValidator<User>[0],
    //     new IPasswordValidator<User>[0],
    //     new Mock<ILookupNormalizer>().Object,
    //     new Mock<IdentityErrorDescriber>().Object,
    //     new Mock<IServiceProvider>().Object,
    //     new Mock<ILogger<UserManager<User>>>().Object)
    //    { }
    //}

    public class ArticlesSeedServiceTests
    {
        //    public ArticlesSeedService(
        //IDeletableEntityRepository<Article> articlesRepository,
        //IDeletableEntityRepository<Supplier> suppliersRepository,
        //IRepository<ArticleSupplier> articleSuppliersRepository,
        //IDeletableEntityRepository<ConformityType> conformityTypesRepository,
        //IDeletableEntityRepository<Conformity> conformitiesRepository,
        //IServiceProvider serviceProvider)
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public ArticlesSeedServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateCorrectlyWithoutSupplier()
        {
            // Arrange
            // create in-memory DB and dbContext
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            // create repos
            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var suppliersRepository = new EfDeletableEntityRepository<Supplier>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformityTypesRepository = new EfDeletableEntityRepository<ConformityType>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);

            // fake admin users IDs
            IList<ApplicationUser> adminUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1"},
                new ApplicationUser { Id = "2"},
                new ApplicationUser { Id = "3"},
            };

            // mock UserManager and its method GetUsersInRoleAsync - needed in the tested service to assign admin user to articles
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                MockBehavior.Default,
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            mockUserManager.Setup(u => u.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName))
                .Returns(Task.FromResult(adminUsers));

            // real service collection to add scoped the mocked UserManager
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<UserManager<ApplicationUser>>(provider => mockUserManager.Object);

            // service provider to provider the service collection with one service in it - the mocked UserManager
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "SVHC" },
                new ConformityType() { Description = "DS_Substances" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesSeedService(
                articlesRepository,
                suppliersRepository,
                articleSuppliersRepository,
                conformityTypesRepository,
                conformitiesRepository,
                serviceProvider);

            var entityToCreate = new ArticleImportDTO()
            {
                Number = "entityToCreateNumber",
                Description = "entityToCreateDescription",
                UserId = "AdminId",
            };

            // Act
            await service.CreateAsync(entityToCreate);

            // Assert
            Assert.Equal(1, articlesRepository.All().Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.Article.Number == "entityToCreateNumber".ToUpper() && x.ConformityTypeId == 1));

            //var userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var adminUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);

            //var articleEntity = this.articlesRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.Number == articleImportDTO.Number.Trim().ToUpper());

            //if (articleEntity != null)
            //{
            //    throw new ArgumentException($"There is already an article with this number.");
            //}

            //var article = new Article
            //{
            //    Number = articleImportDTO.Number.Trim().ToUpper(),
            //    Description = PascalCaseConverter.Convert(articleImportDTO.Description),
            //    User = adminUsers.FirstOrDefault(),
            //};

            //article.ArticleConformityTypes.Add(new ArticleConformityType
            //{
            //    ConformityTypeId = this.conformityTypesRepository
            //                            .AllAsNoTracking()
            //                            .FirstOrDefault(x => x.Description.ToUpper() == "ROHS").Id,
            //});

            //article.ArticleConformityTypes.Add(new ArticleConformityType
            //{
            //    ConformityTypeId = this.conformityTypesRepository
            //                            .AllAsNoTracking()
            //                            .FirstOrDefault(x => x.Description.ToUpper() == "DS_Substances").Id,
            //});

            //article.ArticleConformityTypes.Add(new ArticleConformityType
            //{
            //    ConformityTypeId = this.conformityTypesRepository
            //                            .AllAsNoTracking()
            //                            .FirstOrDefault(x => x.Description.ToUpper() == "SVHC").Id,
            //});

            //await this.articlesRepository.AddAsync(article);

            //await this.articlesRepository.SaveChangesAsync();

            //if (articleImportDTO.SupplierName != null && articleImportDTO.SupplierNumber != null)
            //{
            //    await this.AddSupplierToArticleAsync(article, articleImportDTO);
            //}
        }
    }
}
