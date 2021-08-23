namespace ConformityCheck.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.Extensions.Caching.Distributed;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class SuppliersServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumberOfSuppliers()
        {
            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();

            suppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Supplier>
                             {
                                 new Supplier(),
                                 new Supplier(),
                                 new Supplier(),
                             }.AsQueryable());

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            var count = service.GetCount();

            Assert.Equal(3, count);
            suppliersRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnCorrectNumberOfSuppliersByValidInput()
        {
            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();

            suppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Supplier>
                             {
                                 new Supplier() { Number = "Test12345Number", Name = "Test" },
                                 new Supplier() { Number = "Test123456Number", Name = "Test" },
                                 new Supplier() { Number = "Test12347Number", Name = "Test" },
                                 new Supplier() { Number = "Test12345Number", Name = "Test12345Description" },
                                 new Supplier() { Number = "Test", Name = "Test12345Description" },
                                 new Supplier() { Number = "Test", Name = "Test12347Description" },
                             }.AsQueryable());

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            var count = service.GetCountBySearchInput("12345");

            Assert.Equal(4, count);
            suppliersRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnAllIfSearchInputIsNullOrWhiteSpace()
        {
            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();

            suppliersRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<Supplier>
                             {
                                 new Supplier() { Number = "Test12345Number", Name = "Test" },
                                 new Supplier() { Number = "Test123456Number", Name = "Test" },
                                 new Supplier() { Number = "Test12347Number", Name = "Test" },
                                 new Supplier() { Number = "Test12345Number", Name = "Test12345Description" },
                                 new Supplier() { Number = "Test", Name = "Test12345Description" },
                                 new Supplier() { Number = "Test", Name = "Test12347Description" },
                             }.AsQueryable());

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            var count = service.GetCountBySearchInput(" ");
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput("     ");
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput(string.Empty);
            Assert.Equal(6, count);
            count = service.GetCountBySearchInput(null);
            Assert.Equal(6, count);

            suppliersRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public async Task EditAsyncShouldChangeSupplierDataCorrectly()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();
            var id = Guid.NewGuid().ToString();
            var suppliers = new List<Supplier>
                             {
                                 new Supplier() { Id = id, Number = "Test12345Number", Name = "Test" },
                             };
            suppliersRepository.Setup(r => r.All())
                .Returns(suppliers.AsQueryable().BuildMock().Object);

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            await service.EditAsync(
                new SupplierEditInputModel
                {
                    Id = id,
                    Name = "noname",
                    Email = "test@email.com",
                    ContactPersonFirstName = "ContactPersonFirstName",
                    ContactPersonLastName = "ContactPersonLastName",
                    PhoneNumber = "0359887888888",
                }, userId);

            Assert.Equal("noname".ToUpper(), suppliers.Where(x => x.Id == id).FirstOrDefault().Name);
            Assert.Equal("test@email.com", suppliers.Where(x => x.Id == id).FirstOrDefault().Email);
            Assert.Equal(PascalCaseConverter.Convert("ContactPersonFirstName"), suppliers.Where(x => x.Id == id).FirstOrDefault().ContactPersonFirstName);
            Assert.Equal(PascalCaseConverter.Convert("ContactPersonLastName"), suppliers.Where(x => x.Id == id).FirstOrDefault().ContactPersonLastName);
            Assert.Equal("0359887888888", suppliers.Where(x => x.Id == id).FirstOrDefault().PhoneNumber);
            Assert.Equal("1", suppliers.Where(x => x.Id == id).FirstOrDefault().UserId);
        }

        [Fact]
        public async Task EditAsyncShouldNotChangeSupplierNumber()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();
            var id = Guid.NewGuid().ToString();
            var suppliers = new List<Supplier>
                             {
                                 new Supplier() { Id = id, Number = "Test12345Number", Name = "Test" },
                             };
            suppliersRepository.Setup(r => r.All())
                .Returns(suppliers.AsQueryable().BuildMock().Object);

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            await service.EditAsync(
                new SupplierEditInputModel
                {
                    Id = id,
                    Number = "nonumber",
                }, userId);

            Assert.Equal("Test12345Number", suppliers.Where(x => x.Id == id).FirstOrDefault().Number);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteEntityByProperIdProvided()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
            };
            var userId = users.FirstOrDefault().Id;

            var suppliersRepository = new Mock<IDeletableEntityRepository<Supplier>>();
            var conformitiesRepository = new Mock<IDeletableEntityRepository<Conformity>>();
            var articleSuppliersRepository = new Mock<IRepository<ArticleSupplier>>();
            var distributedCache = new Mock<IDistributedCache>();
            var supplierId = Guid.NewGuid().ToString();

            var suppliers = new List<Supplier>
            {
                new Supplier() { Id = supplierId, Number = "TestNumber1", Name = "TestName1" },
                new Supplier() { Id = Guid.NewGuid().ToString(), Number = "TestNumber2", Name = "TestName2" },
                new Supplier() { Id = Guid.NewGuid().ToString(), Number = "TestNumber3", Name = "TestName3" },
            };
            suppliersRepository.Setup(r => r.All()).Returns(suppliers.AsQueryable().BuildMock().Object);
            suppliersRepository.Setup(r => r.Delete(It.IsAny<Supplier>()))
                .Callback((Supplier supplier) => suppliers.Remove(supplier));

            var articleSuppliers = new List<ArticleSupplier>
            {
                new ArticleSupplier() { Id = 1, ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new ArticleSupplier() { Id = 2, ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new ArticleSupplier() { Id = 3, ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new ArticleSupplier() { Id = 4, ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
                new ArticleSupplier() { Id = 4, ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
            };
            articleSuppliersRepository.Setup(r => r.All()).Returns(articleSuppliers.AsQueryable().BuildMock().Object);
            articleSuppliersRepository.Setup(r => r.Delete(It.IsAny<ArticleSupplier>()))
                .Callback((ArticleSupplier articleSupplier) => articleSuppliers.Remove(articleSupplier));

            var supplierConformities = new List<Conformity>
            {
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString(), SupplierId = supplierId },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
                new Conformity() { Id = Guid.NewGuid().ToString(), ArticleId = Guid.NewGuid().ToString(), SupplierId = Guid.NewGuid().ToString() },
            };
            conformitiesRepository.Setup(r => r.All()).Returns(supplierConformities.AsQueryable().BuildMock().Object);
            conformitiesRepository.Setup(r => r.Delete(It.IsAny<Conformity>()))
                .Callback((Conformity conformity) => supplierConformities.Remove(conformity));

            var service = new SuppliersService(
                suppliersRepository.Object,
                conformitiesRepository.Object,
                articleSuppliersRepository.Object,
                distributedCache.Object);

            await service.DeleteAsync(supplierId, userId);

            Assert.Empty(suppliers.Where(x => x.Id == supplierId));
            Assert.Equal(2, suppliers.Count());
            Assert.Empty(supplierConformities.Where(x => x.Id == supplierId));
            Assert.Equal(2, supplierConformities.Count());
            Assert.Empty(articleSuppliers.Where(x => x.SupplierId == supplierId));
            Assert.Equal(2, articleSuppliers.Count());
        }
    }
}
