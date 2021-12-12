namespace ConformityCheck.Web.ViewModels.Administration.Roles.ViewComponents
{
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class RolesViewComponentModel : IMapFrom<ApplicationRole>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
