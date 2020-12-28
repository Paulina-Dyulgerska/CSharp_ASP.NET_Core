namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityTypeDeleteInputModel
    {
        [ConformityTypeEntityAttribute]
        [ConformityTypeArticlesAttribute]
        public int Id { get; set; }
    }
}
