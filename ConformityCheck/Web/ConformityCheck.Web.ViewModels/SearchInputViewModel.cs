namespace ConformityCheck.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Web.Infrastructure.ValidationAttributes;

    public class SearchInputViewModel
    {
        [Required]
        public string Input { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
