namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public abstract class ArticleBaseModel : IMapFrom<Article>, IMapTo<Article>
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*$", ErrorMessage = "The field Article Nr. could contain only letters, digits or '-'.")]
        [Display(Name = "* Article Nr.:")]
        [ArticleNumberDublicatedAttribute]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        [Display(Name = "* Article description:")]
        //[DescriptionRegExAttribute]
        public string Description { get; set; }
    }
}
