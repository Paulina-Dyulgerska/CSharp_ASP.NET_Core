namespace ConformityCheck.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class UserEditInputModel
    {
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
