namespace ConformityCheck.Web.ViewModels.Conformities
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityIdInputModel
    {
        [ConformityEntityAttribute]
        public string Id { get; set; }
    }
}
