namespace ConformityCheck.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.ContactFormEntries;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Xunit;

    public class ContactFormEntriesServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;

        public ContactFormEntriesServiceTests()
        {
            this.userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateCorrectly()
        {
            var contactFormEntriesRepository = new Mock<IRepository<ContactFormEntry>>();

            var contactFormEntries = new List<ContactFormEntry>
                             {
                                 new ContactFormEntry()
                                 {
                                     Name = "Paulina",
                                     Email = "paulina.dyulgerska@gmail.com",
                                     Title = "Hello",
                                     Content = "I would like to say hi and test this service.",
                                 },
                             };

            var entity = new ContactFormEntryViewModel()
            {
                Name = "Dyulgerska",
                Email = "paulinadyulgerska@gmail.com",
                Title = "Hello",
                Content = "I would like to say hi and test this service.",
                RecaptchaValue = "Dasdasda",
            };

            contactFormEntriesRepository.Setup(r => r.AddAsync(It.IsAny<ContactFormEntry>()))
                .Callback((ContactFormEntry entity) => contactFormEntries.Add(entity));

            var service = new ContactFormEntriesService(contactFormEntriesRepository.Object);

            await service.CreateAsync(entity, "1");

            Assert.Equal(2, contactFormEntries.Count());
            contactFormEntriesRepository.Verify(x => x.AddAsync(It.IsAny<ContactFormEntry>()), Times.Once);
            contactFormEntriesRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.Equal("Dyulgerska", contactFormEntries.Find(x => x.Email == "paulinadyulgerska@gmail.com").Name);
        }
    }
}
