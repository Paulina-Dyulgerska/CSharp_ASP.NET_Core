namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityTypeIdInputModel
    {
        [ConformityTypeEntityAttribute]
        public int Id { get; set; }
    }
}
