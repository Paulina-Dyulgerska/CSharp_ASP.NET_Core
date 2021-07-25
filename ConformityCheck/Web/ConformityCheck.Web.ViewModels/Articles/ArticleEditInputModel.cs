﻿namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleEditInputModel
    {
        [Required]
        [ArticleEntityAttribute]
        public string Id { get; set; }

        // [DescriptionRegExAttribute]
        [Required]
        [MaxLength(50)]
        [Display(Name = "* Article description:")]
        [RegularExpression(
            "^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$",
            ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        public string Description { get; set; }
    }
}
