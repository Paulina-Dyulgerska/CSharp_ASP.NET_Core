namespace ConformityCheck.Web.ViewModels.Conformities
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityEditInputModel : ConformityBaseModel
    {
        [ConformityEntityAttribute(allowNull: true)]
        public string Id { get; set; }

        public string CallerViewName { get; set; }
    }
}
