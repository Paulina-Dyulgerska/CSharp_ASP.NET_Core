namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleBaseInputModel : IMapFrom<Article>, IMapFrom<ArticleSupplier>, IMapTo<Article>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*$", ErrorMessage = "The field Article Nr. could contain only letters, digits or '-'.")]
        [Display(Name = "* Article Nr.:")]
        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        [Display(Name = "* Article description:")]
        //[DescriptionRegExAttribute]
        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleSupplier, ArticleBaseInputModel>()
                .ForMember(
                x => x.Id,
                opt => opt.MapFrom(a => a.ArticleId))
                .ForMember(
                x => x.Description,
                opt => opt.MapFrom(a => a.Article.Description))
                .ForMember(
                x => x.Number,
                opt => opt.MapFrom(a => a.Article.Number));
        }
    }
}
