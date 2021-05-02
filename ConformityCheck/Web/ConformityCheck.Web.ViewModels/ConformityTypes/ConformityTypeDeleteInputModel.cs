namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityTypeDeleteInputModel
    {
        [ConformityTypeEntityAttribute]
        [ConformityTypeUsageAttribute]
        public int Id { get; set; }
    }
}
