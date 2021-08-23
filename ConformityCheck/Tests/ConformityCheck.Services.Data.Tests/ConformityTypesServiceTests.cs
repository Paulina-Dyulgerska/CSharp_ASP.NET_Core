namespace ConformityCheck.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;
    using Xunit;

    public class ConformityTypesServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumberOfConformityTypes()
        {
            var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            conformityTypesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<ConformityType>
                             {
                                 new ConformityType(),
                                 new ConformityType(),
                                 new ConformityType(),
                             }.AsQueryable());

            var service = new ConformityTypesService(
                conformityTypesRepository.Object,
                distributedCache.Object);

            var count = service.GetCount();

            Assert.Equal(3, count);
            conformityTypesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnCorrectNumberOfConformityTypesByValidInput()
        {
            var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            conformityTypesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<ConformityType>
                             {
                                 new ConformityType() { Id = 1, Description = "RoHS" },
                                 new ConformityType() { Id = 2, Description = "REACH" },
                                 new ConformityType() { Id = 3, Description = "FC_EU" },
                                 new ConformityType() { Id = 4, Description = "FC_USA" },
                             }.AsQueryable());

            var service = new ConformityTypesService(
                conformityTypesRepository.Object,
                distributedCache.Object);

            var count = service.GetCountBySearchInput("FC");

            Assert.Equal(2, count);
            conformityTypesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountBySearchInputShouldReturnAllByNullOrWhiteSpaceSearchInput()
        {
            var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
            var distributedCache = new Mock<IDistributedCache>();

            conformityTypesRepository.Setup(r => r.AllAsNoTracking())
                .Returns(new List<ConformityType>
                             {
                                 new ConformityType() { Id = 1, Description = "RoHS" },
                                 new ConformityType() { Id = 2, Description = "REACH" },
                                 new ConformityType() { Id = 3, Description = "FC_EU" },
                                 new ConformityType() { Id = 4, Description = "FC_USA" },
                             }.AsQueryable());

            var service = new ConformityTypesService(
                conformityTypesRepository.Object,
                distributedCache.Object);

            var count = service.GetCountBySearchInput(" ");
            Assert.Equal(4, count);
            count = service.GetCountBySearchInput("     ");
            Assert.Equal(4, count);
            count = service.GetCountBySearchInput(string.Empty);
            Assert.Equal(4, count);
            count = service.GetCountBySearchInput(null);
            Assert.Equal(4, count);

            conformityTypesRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        // [Fact]
        // public async Task CreateAsyncShouldCreateProperlyByValidData()
        // {
        //    var conformityTypesRepository = new Mock<IDeletableEntityRepository<ConformityType>>();
        //    var distributedCache = new Mock<IDistributedCache>();
        //    var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
        //    var cache1 = new Mock<MemoryDistributedCache>(opts);
        //    var conformityTypes = new List<ConformityType>
        //                     {
        //                         new ConformityType() { Id = 1, Description = "RoHS" },
        //                         new ConformityType() { Id = 2, Description = "REACH" },
        //                         new ConformityType() { Id = 3, Description = "FC_EU" },
        //                         new ConformityType() { Id = 4, Description = "FC_USA" },
        //                     };
        //    conformityTypesRepository.Setup(r => r.AllAsNoTracking())
        //        .Returns(conformityTypes.AsQueryable().BuildMock().Object);
        //    var service = new ConformityTypesService(
        //        conformityTypesRepository.Object,
        //        distributedCache.Object);
        //    await service.CreateAsync(new ConformityTypeCreateInputModel { Description = "TestConformityType", }, "1");
        //    var count = service.GetCount();
        //    Assert.Equal(5, count);
        //    conformityTypesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        // }
    }
}
