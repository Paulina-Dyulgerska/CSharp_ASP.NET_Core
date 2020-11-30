namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ArticleEditInputModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        [Display(Name = "* Article description:")]
        public string Description { get; set; }
    }
}
