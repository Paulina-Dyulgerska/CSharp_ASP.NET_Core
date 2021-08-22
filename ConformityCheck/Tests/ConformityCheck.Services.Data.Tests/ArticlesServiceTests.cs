namespace ConformityCheck.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using ConformityCheck.Data;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Data.Repositories;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ArticlesServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public ArticlesServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load("ConformityCheck.Services.Data.Tests"));
        }

        public ArticlesService Before()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var articlesRepository = new EfDeletableEntityRepository<Article>(db);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(db);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(db);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(db);
            var distributedCache = new Mock<IDistributedCache>();
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new ArticlesService(
                                articlesRepository,
                                articleSuppliersRepository,
                                conformitiesRepository,
                                articleConformityTypeRepository,
                                distributedCache.Object);

            return service;
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            // Arrange
            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            articlesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Article>
                             {
                                 new Article(),
                                 new Article(),
                                 new Article(),
                             }.AsQueryable());

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            // Act
            var count = service.GetCount();

            // Assert
            Assert.Equal(3, count);
            articlesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnCorrectNumber()
        {
            // Arrange
            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            articlesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Article>
                             {
                                 new Article() { Number = "Test12345Number", Description = "Test" },
                                 new Article() { Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             }.AsQueryable());

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            // Act
            var count = service.GetCountBySearchInput("12345");

            // Assert
            Assert.Equal(4, count);
            articlesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnAllByNullOrWhiteSpaceSearchInput()
        {
            // Arrange
            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            articlesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Article>
                             {
                                 new Article() { Number = "Test12345Number", Description = "Test" },
                                 new Article() { Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             }.AsQueryable());

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            var count = service.GetCountBySearchInput(" ");
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput("     ");
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput(string.Empty);
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput(null);
            Assert.Equal(6, count);

            articlesRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        //[Fact]
        //public async Task GetByIdShouldReturnCorrectNumber()
        //{
        //    // Arrange
        //    AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load("ConformityCheck.Services.Data.Tests"));
        //    var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
        //    var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
        //    var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
        //    var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
        //    var distributedCache = new Mock<IDistributedCache>();


        //    var articles = new List<Article>
        //                     {
        //                         new Article() { Id = "Test12345Id", Description = "Test" },
        //                         new Article() { Number = "Test123456Number", Description = "Test" },
        //                         new Article() { Number = "Test12347Number", Description = "Test" },
        //                         new Article() { Number = "Test12345Number", Description = "Test12345Description" },
        //                         new Article() { Number = "Test", Description = "Test12345Description" },
        //                         new Article() { Number = "Test", Description = "Test12347Description" },
        //                     }
        //                     //.AsQueryable()
        //                     //.AsAsyncEnumerable()
        //                     ;
        //    var id = "Test12345Id";

        //    articlesRepository.Setup(r => r.All())
        //        .Returns(articles.AsQueryable());

        //    var service = new ArticlesService(
        //        articlesRepository.Object,
        //        articleSuppliersRepository.Object,
        //        conformitiesRepository.Object,
        //        articleConformityTypeRepository.Object,
        //        distributedCache.Object);

        //    // Act
        //    var article = await service.GetByIdAsync<ArticleDetailsExportModel>(id);

        //    // Assert
        //    //Assert.Equal("Test", article);
        //    articlesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        //}

        //[Fact]
        //public async Task GetByIdShouldReturnCorrectNumber()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        //    var db = new ApplicationDbContext(options);
        //    var articlesRepository = new EfDeletableEntityRepository<Article>(db);
        //    var articleSuppliersRepository = new EfRepository<ArticleSupplier>(db);
        //    var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(db);
        //    var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(db);
        //    var distributedCache = new Mock<IDistributedCache>();
        //    var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

        //    var service = new ArticlesService(
        //                        articlesRepository,
        //                        articleSuppliersRepository,
        //                        conformitiesRepository,
        //                        articleConformityTypeRepository,
        //                        distributedCache.Object);
        //    var article = new ArticleCreateInputModel
        //    {

        //         RecaptchaValue = 1,
        //    }
        //    // Act
        //    var article = await service.GetByIdAsync<ArticleDetailsExportModel>(id);

        //    // Assert
        //    //Assert.Equal("Test", article);
        //    articlesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        //}

    }
}