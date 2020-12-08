namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeEditInputModel : ConformityTypeBaseModel, IMapFrom<ConformityType>
    {
        [ConformityTypeEntityAttribute]
        public int Id { get; set; }
    }
}
