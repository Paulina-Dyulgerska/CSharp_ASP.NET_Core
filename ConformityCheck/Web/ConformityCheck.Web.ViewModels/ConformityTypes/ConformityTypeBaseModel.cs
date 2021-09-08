namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public abstract class ConformityTypeBaseModel : IMapFrom<ConformityType>
    {
        // [DescriptionRegExAttribute] // not used - this is how I save a validation request on the FE = 1 request less
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field could contain only letters, digits, '-', '_' or ' '.")]
        [ConformityTypeEntityDescriptionDublicatedAttribute]
        [Display(Name = "* Description")]
        public virtual string Description { get; set; }
    }
}
