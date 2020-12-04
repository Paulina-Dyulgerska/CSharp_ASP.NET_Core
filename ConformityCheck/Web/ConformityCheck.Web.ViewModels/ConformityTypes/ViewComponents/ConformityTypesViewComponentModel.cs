namespace ConformityCheck.Web.ViewModels.Suppliers.ViewComponents
{
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypesViewComponentModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
