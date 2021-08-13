namespace ConformityCheck.Web.ViewModels.Articles
{
    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleIdInputModel
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }
    }
}
