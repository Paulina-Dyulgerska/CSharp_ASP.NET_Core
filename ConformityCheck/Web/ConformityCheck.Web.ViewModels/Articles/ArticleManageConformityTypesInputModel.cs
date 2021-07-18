﻿namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleManageConformityTypesInputModel /*: IValidatableObject*/
    {
        // Required e here to activate the Required in the client-side validation.
        [Required]
        [ArticleEntityAttribute]
        public string Id { get; set; }

        [Required]
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     var context = (IContentCheckService)validationContext
        //         .GetService(typeof(IContentCheckService));
        //     var articleConformityTypeEntity = context
        //         .ArticleConformityTypeEntityIdCheck(this.Id, this.ConformityTypeId);
        //     if (!articleConformityTypeEntity)
        //     {
        //         yield return new ValidationResult("The article does not have such conformity type.");
        //     }
        // }
    }
}
