namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleIdInputModel
    {
        [Required]
        [ArticleEntityAttribute]
        public string Id { get; set; }
    }
}
