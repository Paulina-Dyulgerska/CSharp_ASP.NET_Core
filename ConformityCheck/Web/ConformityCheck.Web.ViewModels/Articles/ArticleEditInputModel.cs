namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleEditInputModel
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "* Article description:")]
        [DescriptionRegExAttribute]
        public string Description { get; set; }
    }
}
