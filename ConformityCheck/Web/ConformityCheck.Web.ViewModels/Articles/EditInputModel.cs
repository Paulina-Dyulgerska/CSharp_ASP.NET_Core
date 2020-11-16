namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditInputModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        [Display(Name = "Article description")]
        public string Description { get; set; }

        public string SupplierId { get; set; }

        [Display(Name = "Select conformity types")]
        public IEnumerable<int> ConformityTypes { get; set; }
    }
}
