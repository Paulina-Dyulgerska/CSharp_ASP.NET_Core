namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleBaseModel : IMapFrom<Article>, IMapTo<Article>
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        [Display(Name = "Article Nr.:")]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        [Display(Name = "Article description:")]
        public string Description { get; set; }
    }
}
