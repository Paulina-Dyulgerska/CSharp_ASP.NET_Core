namespace ConformityCheck.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Data.Repositories;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ArticlesServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public ArticlesServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        // public ArticlesService Before()
        // {
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
        //    return service;
        // }
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

        [Fact]
        public async Task CreateAsyncShouldCreateCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var supplierId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                             };

            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entityToCreate = new ArticleCreateInputModel()
            {
                Number = "entityToCreateNumber",
                Description = "entityToCreateDescription",
                SupplierId = supplierId,
                ConformityTypes = new int[] { 1, 3 },
                RecaptchaValue = "recaptcha",
            };

            // Act
            await service.CreateAsync(entityToCreate, "1");

            // Assert
            Assert.Equal(1, articleSuppliersRepository.All().Count());
            Assert.Equal(supplierId, articleSuppliersRepository.All().FirstOrDefault().SupplierId);
            Assert.Equal(2, articleConformityTypeRepository.All().Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.Article.Number == "entityToCreateNumber".ToUpper() && x.ConformityTypeId == 1));
            Assert.True(articleConformityTypeRepository.All().Any(x => x.Article.Number == "entityToCreateNumber".ToUpper() && x.ConformityTypeId == 3));
            Assert.Equal(1, articlesRepository.All().Where(x => x.Number == "entityToCreateNumber".ToUpper()).Count());
            Assert.Single(articlesRepository.All().Where(x => x.Number == "entityToCreateNumber".ToUpper()).FirstOrDefault().ArticleSuppliers);
            Assert.Equal(
                "Entitytocreatedescription",
                articlesRepository.All().Where(x => x.Number == "entityToCreateNumber".ToUpper()).FirstOrDefault().Description);
        }

        [Fact]
        public async Task CreateAsyncShouldThrowErrorByDublicatedArticleNumber()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var supplierId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                             };

            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entityToCreate = new ArticleCreateInputModel()
            {
                Number = "Test12347Number",
                Description = "entityToCreateDescription",
                SupplierId = supplierId,
                ConformityTypes = new int[] { 1, 3 },
                RecaptchaValue = "recaptcha",
            };

            // Act
            await service.CreateAsync(entityToCreate, "1");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityToCreate, "1"));
        }

        [Fact]
        public async Task EditAsyncShouldChangeArticleDataCorrectly()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            var id = Guid.NewGuid().ToString();
            var articles = new List<Article>
                             {
                                 new Article() { Number = "Test12345Number", Description = "Test" },
                                 new Article() { Id = id, Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             };

            articlesRepository.Setup(r => r.All())
                .Returns(articles.AsQueryable().BuildMock().Object);

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.EditAsync(
                new ArticleEditInputModel
                {
                    Id = id,
                    Description = "noname",
                }, userId);

            Assert.Equal(PascalCaseConverter.Convert("noname"), articles.Where(x => x.Id == id).FirstOrDefault().Description);
        }

        [Fact]
        public async Task DeleteAsyncShouldRemoveArticleAndAllItsSuppliersConformitiesAndConformityTypes()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Number = "Test12345Number", Description = "Test" },
                                 new Article() { Id = articleId, Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             };
            articlesRepository.Setup(r => r.All()).Returns(articles.AsQueryable().BuildMock().Object);
            articlesRepository.Setup(r => r.Delete(It.IsAny<Article>())).Callback((Article article) => articles.Remove(article));

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
            };
            articleSuppliersRepository.Setup(r => r.All()).Returns(articleSuppliers.AsQueryable().BuildMock().Object);
            articleSuppliersRepository.Setup(r => r.Delete(It.IsAny<ArticleSupplier>()))
                .Callback((ArticleSupplier articleSupplier) => articleSuppliers.Remove(articleSupplier));

            var articleConformityTypes = new List<ArticleConformityType>
            {
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = Guid.NewGuid().ToString(), ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = Guid.NewGuid().ToString(), ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 3 },
                new ArticleConformityType() { ArticleId = Guid.NewGuid().ToString(), ConformityTypeId = 1 },
            };
            articleConformityTypeRepository.Setup(r => r.All()).Returns(articleConformityTypes.AsQueryable().BuildMock().Object);
            articleConformityTypeRepository.Setup(r => r.Delete(It.IsAny<ArticleConformityType>()))
                .Callback((ArticleConformityType articleConformityType) => articleConformityTypes.Remove(articleConformityType));

            var articleConformities = new List<Conformity>
            {
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString() },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId },
            };
            conformitiesRepository.Setup(r => r.All()).Returns(articleConformities.AsQueryable().BuildMock().Object);
            conformitiesRepository.Setup(r => r.Delete(It.IsAny<Conformity>()))
                .Callback((Conformity conformity) => articleConformities.Remove(conformity));

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.DeleteAsync(articleId, userId);

            Assert.Empty(articles.Where(x => x.Id == articleId));
            Assert.Equal(5, articles.Count());
            Assert.Empty(articleSuppliers.Where(x => x.ArticleId == articleId));
            Assert.Equal(2, articleSuppliers.Count());
            Assert.Empty(articleConformityTypes.Where(x => x.ArticleId == articleId));
            Assert.Equal(3, articleConformityTypes.Count());
            Assert.Empty(articleConformities.Where(x => x.ArticleId == articleId));
            Assert.Single(articleConformities);
        }

        [Fact]
        public async Task AddSuppliersyncShouldAddCorrectly()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplierId = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
            };
            articleSuppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            articleSuppliersRepository.Setup(r => r.AddAsync(It.IsAny<ArticleSupplier>()))
                .Callback((ArticleSupplier articleSupplier) => articleSuppliers.Add(articleSupplier));

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.AddSupplierAsync(new ArticleManageSuppliersInputModel
            {
                Id = articleId,
                SupplierId = supplierId,
            });

            Assert.Single(articleSuppliers.Where(x => x.ArticleId == articleId));
            Assert.Equal(2, articleSuppliers.Count());
            Assert.True(articleSuppliers.Where(x => x.ArticleId == articleId).FirstOrDefault().IsMainSupplier);
        }

        [Fact]
        public async Task AddSuppliersyncShouldNotAddAlreadyAddedSupplier()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplierId = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplierId, IsMainSupplier = true },
            };
            articleSuppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            articleSuppliersRepository.Setup(r => r.AddAsync(It.IsAny<ArticleSupplier>()))
                .Callback((ArticleSupplier articleSupplier) => articleSuppliers.Add(articleSupplier));

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.AddSupplierAsync(new ArticleManageSuppliersInputModel
            {
                Id = articleId,
                SupplierId = supplierId,
            });

            Assert.Single(articleSuppliers.Where(x => x.ArticleId == articleId));
            Assert.Single(articleSuppliers.Where(x => x.SupplierId == supplierId));
            Assert.Equal(2, articleSuppliers.Count());
            Assert.True(articleSuppliers.Where(x => x.ArticleId == articleId).FirstOrDefault().IsMainSupplier);
        }

        [Fact]
        public async Task AddSuppliersyncShouldNotAddMainSupplierIfAlreadyHasSupplierAddedToArticle()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplierId = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = Guid.NewGuid().ToString(), IsMainSupplier = true },
            };
            articleSuppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            articleSuppliersRepository.Setup(r => r.AddAsync(It.IsAny<ArticleSupplier>()))
                .Callback((ArticleSupplier articleSupplier) => articleSuppliers.Add(articleSupplier));

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.AddSupplierAsync(new ArticleManageSuppliersInputModel
            {
                Id = articleId,
                SupplierId = supplierId,
            });

            Assert.Equal(2, articleSuppliers.Where(x => x.ArticleId == articleId).Count());
            Assert.Equal(3, articleSuppliers.Count());
            Assert.False(articleSuppliers.Where(x => x.ArticleId == articleId && x.SupplierId == supplierId)
                                            .FirstOrDefault().IsMainSupplier);
        }

        [Fact]
        public async Task ChangeMainSupplierShoutChangeExistingSuppliersCorrectly()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplier1Id = Guid.NewGuid().ToString();
            var supplier2Id = Guid.NewGuid().ToString();
            var supplier3Id = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier1Id, IsMainSupplier = true },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier2Id },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier3Id },
            };
            articleSuppliersRepository.Setup(r => r.All())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            await service.ChangeMainSupplierAsync(new ArticleManageSuppliersInputModel
            {
                Id = articleId,
                SupplierId = supplier2Id,
            });

            Assert.Equal(3, articleSuppliers.Where(x => x.ArticleId == articleId).Count());
            Assert.False(articleSuppliers.Where(x => x.ArticleId == articleId && x.SupplierId == supplier1Id)
                                            .FirstOrDefault().IsMainSupplier);
            Assert.False(articleSuppliers.Where(x => x.ArticleId == articleId && x.SupplierId == supplier3Id)
                                .FirstOrDefault().IsMainSupplier);
            Assert.True(articleSuppliers.Where(x => x.ArticleId == articleId && x.SupplierId == supplier2Id)
                                .FirstOrDefault().IsMainSupplier);
        }

        [Fact]
        public async Task RemoveSupplierAsyncShouldRemoveCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplierId = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
                             {
                                 new ArticleSupplier() { ArticleId = articleId, SupplierId = supplierId },
                                 new ArticleSupplier() { ArticleId = articleId, SupplierId = "2" },
                                 new ArticleSupplier() { ArticleId = articleId, SupplierId = "3" },
                                 new ArticleSupplier() { ArticleId = "2", SupplierId = supplierId },
                             };

            dbContext.ArticleSuppliers.AddRange(articleSuppliers);
            await dbContext.SaveChangesAsync();

            var conformities = new List<Conformity>
            {
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString() },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId, SupplierId = supplierId, ConformityTypeId = 1, IsAccepted = true, ValidityDate = DateTime.UtcNow.AddDays(3) },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId, SupplierId = supplierId, ConformityTypeId = 3, IsAccepted = true, ValidityDate = DateTime.UtcNow.AddDays(3) },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = "3", SupplierId = supplierId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId, SupplierId = "3" },
            };
            dbContext.Conformities.AddRange(conformities);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entityToRemove = new ArticleManageSuppliersInputModel()
            {
                Id = articleId,
                SupplierId = supplierId,
            };

            // Act
            await service.RemoveSupplierAsync(entityToRemove);

            // Assert
            Assert.Equal(3, articleSuppliersRepository.All().Count());
            Assert.False(articleSuppliersRepository.All().Any(x => x.ArticleId == articleId && x.SupplierId == supplierId));
            Assert.True(articleSuppliersRepository.All().Any(x => x.ArticleId == articleId));
            Assert.True(articleSuppliersRepository.All().Any(x => x.SupplierId == supplierId));
            Assert.False(conformitiesRepository.All().Any(x => x.ArticleId == articleId && x.SupplierId == supplierId));
            Assert.True(conformitiesRepository.All().Any(x => x.ArticleId == articleId));
            Assert.True(conformitiesRepository.All().Any(x => x.SupplierId == supplierId));
        }

        [Fact]
        public async Task AddConformityTypeAsyncShouldAddCorrectlyNewType()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Number = "Test12347Number", Description = "Test" },
                                 new Article() { Id = "2", Number = "Test12345Number", Description = "Test12345Description" },
                             };
            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var articleToAddEntriesTo = new ArticleManageConformityTypesInputModel
            {
                Id = articleId,
                ConformityTypeId = 1,
            };

            // Act
            await service.AddConformityTypeAsync(articleToAddEntriesTo);

            // Assert
            Assert.Equal(1, articleConformityTypeRepository.All().Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 1));
            Assert.False(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 3));
        }

        [Fact]
        public async Task AddConformityTypesAsyncShouldAddCorrectlyNewTypes()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Number = "Test12347Number", Description = "Test" },
                                 new Article() { Id = "2", Number = "Test12345Number", Description = "Test12345Description" },
                             };
            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entriesToAdd = new int[] { 1, 3 };
            var articleToAddEntriesTo = new Article
            {
                Id = articleId,
            };

            // Act
            await service.AddConformityTypesAsync(articleToAddEntriesTo, entriesToAdd);

            // Assert
            Assert.Equal(2, articleConformityTypeRepository.All().Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 1));
            Assert.True(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 3));
        }

        [Fact]
        public async Task AddConformityTypesAsyncShouldNotAddExistingTypes()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Number = "Test12347Number", Description = "Test" },
                                 new Article() { Id = "2", Number = "Test12345Number", Description = "Test12345Description" },
                             };
            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var articleConformityTypes = new List<ArticleConformityType>
            {
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 2 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 3 },
            };
            dbContext.ArticleConformityTypes.AddRange(articleConformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entriesToAdd = new int[] { 1, 3 };
            var articleToAddEntriesTo = new Article
            {
                Id = articleId,
            };

            // Act
            await service.AddConformityTypesAsync(articleToAddEntriesTo, entriesToAdd);

            // Assert
            Assert.Equal(3, articleConformityTypeRepository.All().Count());
            Assert.Equal(1, articleConformityTypeRepository.All().Where(x => x.ArticleId == articleId && x.ConformityTypeId == 1).Count());
            Assert.Equal(1, articleConformityTypeRepository.All().Where(x => x.ArticleId == articleId && x.ConformityTypeId == 3).Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 3));
        }

        [Fact]
        public async Task AddConformityTypesAsyncShouldNotAddNonexistingTypes()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Number = "Test12347Number", Description = "Test" },
                                 new Article() { Id = "2", Number = "Test12345Number", Description = "Test12345Description" },
                             };
            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var articleConformityTypes = new List<ArticleConformityType>
            {
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 2 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 3 },
            };
            dbContext.ArticleConformityTypes.AddRange(articleConformityTypes);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            var entriesToAdd = new int[] { 4, 5 };
            var articleToAddEntriesTo = new Article
            {
                Id = articleId,
            };

            // Act
            await service.AddConformityTypesAsync(articleToAddEntriesTo, entriesToAdd);

            // Assert
            Assert.Equal(3, articleConformityTypeRepository.All().Count());
            Assert.Equal(1, articleConformityTypeRepository.All().Where(x => x.ArticleId == articleId && x.ConformityTypeId == 1).Count());
            Assert.Equal(1, articleConformityTypeRepository.All().Where(x => x.ArticleId == articleId && x.ConformityTypeId == 3).Count());
            Assert.True(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && x.ConformityTypeId == 3));
            Assert.False(articleConformityTypeRepository.All().Any(x => x.ArticleId == articleId && (x.ConformityTypeId == 4 || x.ConformityTypeId == 5)));
        }

        [Fact]
        public async Task GetSuppliersByIdAsyncShouldReturnCorrectlyByExistingSuppliers()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplier1Id = Guid.NewGuid().ToString();
            var supplier2Id = Guid.NewGuid().ToString();
            var supplier3Id = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier1Id, IsMainSupplier = false, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier2Id, IsMainSupplier = true, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier3Id, IsMainSupplier = false, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
            };
            articleSuppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            var suppliers = await service.GetSuppliersByIdAsync<SupplierExportModel>(articleId);

            Assert.Equal(3, suppliers.Count());
            Assert.True(suppliers.Where(x => x.Id == supplier2Id).FirstOrDefault().IsMainSupplier);
            Assert.False(suppliers.Where(x => x.Id == supplier1Id).FirstOrDefault().IsMainSupplier);
            Assert.False(suppliers.Where(x => x.Id == supplier3Id).FirstOrDefault().IsMainSupplier);
        }

        [Fact]
        public async Task GetSuppliersByIdAsyncShouldReturnEmptyListByNoExistingSuppliers()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var articleId = Guid.NewGuid().ToString();
            var supplier1Id = Guid.NewGuid().ToString();
            var supplier2Id = Guid.NewGuid().ToString();
            var supplier3Id = Guid.NewGuid().ToString();

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier1Id, IsMainSupplier = false, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier2Id, IsMainSupplier = true, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = articleId, SupplierId = supplier3Id, IsMainSupplier = false, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
                new ArticleSupplier() { ArticleId = Guid.NewGuid().ToString(), IsMainSupplier = true, SupplierId = supplier3Id, Supplier = new Supplier { Name = "TestSupplier" } },
            };
            articleSuppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(articleSuppliers.AsQueryable().BuildMock().Object);

            var service = new ArticlesService(
                articlesRepository.Object,
                articleSuppliersRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object);

            var suppliers = await service.GetSuppliersByIdAsync<SupplierExportModel>(Guid.NewGuid().ToString());

            Assert.Empty(suppliers);
        }

        [Fact]
        public async Task GetConformityTypesByIdAndSupplierAsyncReturnCorrectlyByExistingSuppliersAndConformityTypes()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();

            var articleId = Guid.NewGuid().ToString();
            var supplierId = Guid.NewGuid().ToString();

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Description = "Test" },
                                 new Article() { Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             };
            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var conformityTypes = new List<ConformityType>
            {
                new ConformityType() { Description = "RoHS" },
                new ConformityType() { Description = "Reach" },
                new ConformityType() { Description = "FC_EU" },
            };
            dbContext.ConformityTypes.AddRange(conformityTypes);
            await dbContext.SaveChangesAsync();

            var articleConformityTypes = new List<ArticleConformityType>
            {
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 1 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 2 },
                new ArticleConformityType() { ArticleId = articleId, ConformityTypeId = 3 },
            };
            dbContext.ArticleConformityTypes.AddRange(articleConformityTypes);
            await dbContext.SaveChangesAsync();

            var conformities = new List<Conformity>
            {
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString() },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId, SupplierId = supplierId, ConformityTypeId = 1, IsAccepted = true, ValidityDate = DateTime.UtcNow.AddDays(3) },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId, SupplierId = supplierId, ConformityTypeId = 3, IsAccepted = true, ValidityDate = DateTime.UtcNow.AddDays(3) },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = articleId },
            };
            dbContext.Conformities.AddRange(conformities);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            // Act
            var result = await service.GetConformityTypesByIdAndSupplierAsync(articleId, supplierId);

            Assert.Equal(3, result.Count());
            Assert.False(result.Where(x => x.Id == 2).FirstOrDefault().SupplierConfirmed);
            Assert.False(result.Where(x => x.Id == 2).FirstOrDefault().SupplierConfirmed);
            Assert.True(result.Where(x => x.Id == 1).FirstOrDefault().SupplierConfirmed);
            Assert.True(result.Where(x => x.Id == 3).FirstOrDefault().SupplierConfirmed);
        }

        [Theory]
        [InlineData("Test12345Id", "Test")]
        [InlineData("", null)]
        [InlineData(" ", null)]
        public async Task GetByIdShouldReturnCorrectNumber(string articleIdToFind, string description)
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
            var articleSuppliersRepository = new EfRepository<ArticleSupplier>(dbContext);
            var conformitiesRepository = new EfDeletableEntityRepository<Conformity>(dbContext);
            var articleConformityTypeRepository = new EfRepository<ArticleConformityType>(dbContext);
            var distributedCache = new Mock<IDistributedCache>();

            var articleId = articleIdToFind;

            var articles = new List<Article>
                             {
                                 new Article() { Id = articleId, Description = description },
                                 new Article() { Number = "Test123456Number", Description = "Test" },
                                 new Article() { Number = "Test12347Number", Description = "Test" },
                                 new Article() { Number = "Test12345Number", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12345Description" },
                                 new Article() { Number = "Test", Description = "Test12347Description" },
                             };

            dbContext.Articles.AddRange(articles);
            await dbContext.SaveChangesAsync();

            var service = new ArticlesService(
                articlesRepository,
                articleSuppliersRepository,
                conformitiesRepository,
                articleConformityTypeRepository,
                distributedCache.Object);

            // Act
            var article = await service.GetByIdAsync<ArticleDetailsExportModel>(articleId);

            // Assert
            Assert.Equal(articleId, article.Id);
            Assert.Equal(description, article.Description);
        }
    }
}
