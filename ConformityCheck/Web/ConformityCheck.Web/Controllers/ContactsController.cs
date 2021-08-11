namespace ConformityCheck.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Messaging;
    using ConformityCheck.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : BaseController
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";

        private readonly IRepository<ContactFormEntry> contactsRepository;

        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactsController(
            IRepository<ContactFormEntry> contactsRepository,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null)
            {
                var model = new ContactFormViewModel
                {
                    Name = user.FirstName != null ? $"{user.FirstName} {user.LastName}" : user.UserName,
                    Email = user.Email,
                };

                return this.View(model);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // TODO: Extract to IP provider (service)
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            var contactFormEntry = new ContactFormEntry
            {
                Name = model.Name,
                Email = model.Email,
                Title = model.Title,
                Content = model.Content,
                Ip = ip,
            };
            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();

            // send email to site admin
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                $"You have message from {model.Name}",
                $"Message content: {model.Title} - {model.Content}");

            // TODO - user special email and thank you email + thank you page
            // send email to contact message sender
            var user = await this.userManager.GetUserAsync(this.User);
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                model.Email,
                model.Name,
                "Thank you for your message",
                $"Dear {model.Name},\r\n Thank you for your interest to our site and the message sent!\r\n We will contact you as soon as we review your request.\r\n\r\nKind Regards,\r\nConformity Check Team");

            this.TempData[RedirectedFromContactForm] = true;

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        public IActionResult ThankYou()
        {
            if (this.TempData[RedirectedFromContactForm] == null)
            {
                // return this.NotFound();
                // return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return this.View();
        }
    }
}