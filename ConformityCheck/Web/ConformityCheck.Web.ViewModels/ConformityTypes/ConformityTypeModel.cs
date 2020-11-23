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
        public string Description { get; set; }
    }
}
