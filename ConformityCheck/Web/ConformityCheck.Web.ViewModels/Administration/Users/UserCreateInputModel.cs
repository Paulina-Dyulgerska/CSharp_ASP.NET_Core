namespace ConformityCheck.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Web.ViewModels.Administration.Roles.ViewComponents;

    public class UserCreateInputModel
    {
        [Required]
        [EmailAddress]
        [EmailRegExAttribute(ErrorMessage = "Username field is required.")]
        [Display(Name = "* Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "* Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "* Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmedPassword { get; set; }

        [Required]
        [EmailAddress]
        [EmailRegExAttribute]
        [Display(Name = "* User email")]
        public string Email { get; set; }

        [NameRegExAttribute]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [NameRegExAttribute]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is user's email confirmed?")]
        public bool EmailConfirmed { get; set; }

        // [RoleEntityAttribute]
        public IEnumerable<string> Roles { get; set; }
    }
}
