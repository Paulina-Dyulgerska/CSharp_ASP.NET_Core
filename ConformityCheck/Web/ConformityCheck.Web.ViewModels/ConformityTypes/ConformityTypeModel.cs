namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        public string Description { get; set; }
    }
}
