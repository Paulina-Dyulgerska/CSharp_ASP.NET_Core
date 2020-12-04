namespace ConformityCheck.Web.ViewModels.ConformityTypes.ViewComponents
{
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypesViewComponentModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
