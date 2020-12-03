namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class ConformityCreateInputModel : IValidatableObject
    {
        [Required]
        [Display(Name = "* Conformity type:")]
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        [Required]
        [Display(Name = "* Supplier:")]
        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        public bool ValidForAllArticles { get; set; }

        public bool ValidForSingleArticle { get; set; }

        [Display(Name = "* Article:")]
        [ArticleEntityAttribute(allowNull: true)]
        public string ArticleId { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "* Issue date:")]
        [DateAttribute(minDate: "01/01/2000")]
        public DateTime IssueDate { get; set; }

        // [[Already have accepted conformity in the DB???]] Check and validate if there is one?
        public bool IsAccepted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Validity date:")]
        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        // [Required]
        // [FileExtensionAttribute(extension: "pdf")]
        // [FileSize(size: 10 * 1024 * 1024)]
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
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
