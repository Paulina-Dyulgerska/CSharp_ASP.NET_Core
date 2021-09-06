namespace ConformityCheck.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Azure.Storage.Blobs;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Caching.Distributed;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ConformitiesServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public ConformitiesServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            // Arrange
            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var blobServiceClient = new Mock<BlobServiceClient>();

            conformitiesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Conformity>
                             {
                                 new Conformity(),
                                 new Conformity(),
                                 new Conformity(),
                             }.AsQueryable());

            var service = new ConformitiesService(
                articlesRepository.Object,
                suppliersRepository.Object,
                articleSuppliersRepository.Object,
                conformityTypesRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object,
                blobServiceClient.Object);

            // Act
            var count = service.GetCount();

            // Assert
            Assert.Equal(3, count);
            conformitiesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnCorrectNumber()
        {
            var articlesRepository = new Mock<IDeletableEntityRepository<Article>>();
            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleConformityTypeRepository = new Mock<IRepository<ArticleConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();
            var blobServiceClient = new Mock<BlobServiceClient>();

            var conformities = new List<Conformity>
                             {
                                 new Conformity()
                                 {
                                     Article = new Article { Number = "TestArticleNumber1", Description = "TestArticleDescription1" },
                                     Supplier = new Supplier { Number = "TestSupplierNumber1", Name = "TestSupplierName1" },
                                     ConformityType = new ConformityType { Description = "TestConformityTypeDescription1" },
                                 },
                                 new Conformity()
                                 {
                                     Article = new Article { Number = "TestArticleNumber42", Description = "TestArticleDescription2" },
                                     Supplier = new Supplier { Number = "TestSupplierNumber2", Name = "TestSupplierName2" },
                                     ConformityType = new ConformityType { Description = "TestConformityTypeDescription3" },
                                 },
                                 new Conformity()
                                 {
                                     Article = new Article { Number = "TestArticleNumber32", Description = "TestArticleDescription3" },
                                     Supplier = new Supplier { Number = "TestSupplierNumber3", Name = "TestSupplierName3" },
                                     ConformityType = new ConformityType { Description = "TestConformityTypeDescription31" },
                                 },
                                 new Conformity()
                                 {
                                     Article = new Article { Number = "TestArticleNumber42", Description = "TestArticleDescription4" },
                                     Supplier = new Supplier { Number = "TestSupplierNumber4", Name = "TestSupplierName43" },
                                     ConformityType = new ConformityType { Description = "TestConformityTypeDescription4" },
                                 },
                             };

            conformitiesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(conformities.AsQueryable().BuildMock().Object);

            var service = new ConformitiesService(
                articlesRepository.Object,
                suppliersRepository.Object,
                articleSuppliersRepository.Object,
                conformityTypesRepository.Object,
                conformitiesRepository.Object,
                articleConformityTypeRepository.Object,
                distributedCache.Object,
                blobServiceClient.Object);

            var count = service.GetCountBySearchInput("1");
            Assert.Equal(2, count);
            conformitiesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            count = service.GetCountBySearchInput("2");
            Assert.Equal(3, count);
            conformitiesRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));

            count = service.GetCountBySearchInput("3");
            Assert.Equal(3, count);

            count = service.GetCountBySearchInput("4");
            Assert.Equal(2, count);

            count = service.GetCountBySearchInput("32");
            Assert.Equal(1, count);

            count = service.GetCountBySearchInput("31");
            Assert.Equal(1, count);

            count = service.GetCountBySearchInput("Test");
            Assert.Equal(4, count);
            conformitiesRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(7));
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
    }
}
