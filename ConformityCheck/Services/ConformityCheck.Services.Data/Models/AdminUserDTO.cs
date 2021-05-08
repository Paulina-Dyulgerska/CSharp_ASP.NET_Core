namespace ConformityCheck.Services.Data.Models
{
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class AdminUserDTO : IMapTo<ApplicationUser>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
