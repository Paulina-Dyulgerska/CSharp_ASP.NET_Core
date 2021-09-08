namespace ConformityCheck.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Messaging;
    using ConformityCheck.Web.ViewModels.ContactFormEntries;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class ContactFormEntriesController : BaseController
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";

        private readonly IContactFormEntriesService contactFormEntriesService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ContactFormEntriesController> logger;

        public ContactFormEntriesController(
            IContactFormEntriesService contactFormEntriesService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            ILogger<ContactFormEntriesController> logger)
        {
            this.contactFormEntriesService = contactFormEntriesService;
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null)
            {
                var model = new ContactFormEntryViewModel
                {
                    Name = user.FirstName != null ? $"{user.FirstName} {user.LastName}" : user.UserName,
                    Email = user.Email,
                };

                return this.View(model);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormEntryViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                // TODO: Extract to IP provider (service)
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                input.Ip = ip;

                await this.contactFormEntriesService.CreateAsync(input, user?.Id);

                this.logger.LogInformation($"Contact message from {input.Email} created.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Contact message creation failed: {ex}.");
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
            }

            try
            {
                // send email to site admin
                await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    $"You have message from {input.Name}",
                    $"Message content: {input.Title} - {input.Content}");

                this.logger.LogInformation($"Send contact message from {input.Email} to site admin.");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Error sending contact message to site admin: {ex}.");
            }

            try
            {
                // send email to contact message sender
                await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    input.Email,
                    input.Name,
                    "Thank you for your message",
                    $"Dear {input.Name},\r\n Thank you for your interest on our site and the message sent!\r\n We will contact you as soon as we review your request.\r\n\r\nKind Regards,\r\nConformity Check Team");

                this.logger.LogInformation($"Send contact message from site admin to {input.Email}.");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Error sending contact message to user: {ex}.");
            }

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
