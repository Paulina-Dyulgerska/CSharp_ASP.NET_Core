namespace ConformityCheck.Web.ViewModels.ContactFormEntries
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Web.Infrastructure.ValidationAttributes;

    public class ContactFormEntryViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name")]
        [Display(Name = "* Your name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter your email address")]
        [Display(Name = "* Your email address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter message title")]
        [StringLength(
            100,
            ErrorMessage = "The message titel must be at least {2} and no more than {1} symbols long",
            MinimumLength = 5)]
        [Display(Name = "* Message title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter message content")]
        [StringLength(10000, ErrorMessage = "The message must be at least {2} symbols long", MinimumLength = 20)]
        [Display(Name = "* Message content")]
        public string Content { get; set; }

        public string Ip { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
