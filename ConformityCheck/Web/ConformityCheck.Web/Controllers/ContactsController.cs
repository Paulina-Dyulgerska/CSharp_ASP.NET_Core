namespace ConformityCheck.Web.Controllers
{
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

        public IActionResult Index()
        {
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

            var user = await this.userManager.GetUserAsync(this.User);

            // send email to site admin
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                $"{model.Email} - {model.Name}",
                $"{model.Title} - {model.Content}");

            // TODO - user special email and thank you emai + thank you page
            //if (user != null)
            //{
            //    // send email to contact message sender (logged in user)
            //    await this.emailSender.SendEmailAsync(
            //        GlobalConstants.SystemEmail,
            //        GlobalConstants.SystemName,
            //        model.Email,
            //        model.Name,
            //        model.Title,
            //        model.Content,
            //        user.Id);
            //}
            //else
            //{
            //    // send email to contact message sender (not logged in)
            //    await this.emailSender.SendEmailAsync(
            //        GlobalConstants.SystemEmail,
            //        GlobalConstants.SystemName,
            //        model.Email,
            //        model.Name,
            //        model.Title,
            //        model.Content);
            //}

            this.TempData[RedirectedFromContactForm] = true;

            // return this.RedirectToAction("ThankYou");
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult ThankYou()
        {
            if (this.TempData[RedirectedFromContactForm] == null)
            {
                return this.NotFound();
            }

            return this.View();
        }
    }
}