namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Services;
    using Microsoft.AspNetCore.Http;

    public class ConformityCreateInputModel : ConformityBaseModel
    {
        [Required]
        [FileExtensionAttribute(extension: "pdf")]
        [FileSizeAttribute(size: 10 * 1024 * 1024)]
        [Display(Name = "* Conformity file:")]
        public IFormFile InputFile { get; set; }

        public bool ValidForAllArticles { get; set; }

        public bool ValidForSingleArticle { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));
            var articleSupplierEntity = context.ArticleSupplierEntityIdCheck(this.ArticleId, this.SupplierId);

            if (!articleSupplierEntity && this.ArticleId != null)
            {
                yield return new ValidationResult("The article does not have such supplier.");
            }

            if ((this.IssueDate > this.ValidityDate) && (this.ValidityDate != null))
            {
                yield return new ValidationResult("Issue date could not be after the validity date.");
            }

            if (this.ValidForSingleArticle && this.ArticleId == null)
            {
                yield return new ValidationResult("Please choose single article or change the conformity coverage.");
            }

            if (this.ValidForAllArticles && this.ArticleId != null)
            {
                yield return new ValidationResult("Please choose single article or change the conformity coverage.");
            }

            if (this.ValidForSingleArticle && this.ValidForAllArticles)
            {
                yield return new ValidationResult("Please choose one of the options for conformity coverage.");
            }

            if (!this.ValidForSingleArticle && !this.ValidForAllArticles)
            {
                yield return new ValidationResult("Please choose one of the options for conformity coverage.");
            }
        }
    }
}
