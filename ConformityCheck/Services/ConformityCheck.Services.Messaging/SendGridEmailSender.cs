﻿namespace ConformityCheck.Services.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConformityCheck.Common;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;

        public SendGridEmailSender(string apiKey)
        {
            this.client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string toName,
            string subject,
            string htmlContent,
            string userId = null,
            IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to, toName);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            try
            {
                var response = await this.client.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // just to test the email sending:
            // var apiKey = "[Put-Send-Grid-ApiKey-In-ApiKeys-Place]";
            // var client = new SendGridClient(apiKey);
            // var fromA = new EmailAddress("admin@dotnetweb.net", "Example User");
            // var subjectA = "Hello Djudja";
            // var toA = new EmailAddress("paylina_st@yahoo.com", "Example User");
            // var plainTextContent = "Sending emails is not so easy to do, but I have done it, even with C# :)";
            // var htmlContentA = "<strong>Sending emails is not so easy to do, but I have done it, even with C# :)</strong>";
            // var msg = MailHelper.CreateSingleEmail(fromA, toA, subjectA, plainTextContent, htmlContentA);
            // var response = await client.SendEmailAsync(msg);
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            await this.SendEmailAsync(
                GlobalConstants.SystemEmail,
                GlobalConstants.SystemName,
                to,
                to,
                subject,
                htmlContent);
        }
    }
}
