namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ConformityCreateModel : IValidatableObject
    {
        [Required]
        [Display(Name = "* Conformity type:")]
        //[ConformimtyTypeEntityAttribute] 
        public int ConformityTypeId { get; set; }

        [Required]
        [SupplierEntityAttribute]
        [Display(Name = "* Supplier:")]
        public string SupplierId { get; set; }

        public bool ValidForAllArticles { get; set; }

        public bool ValidForSingleArticle { get; set; }

        //[ArticleEntityAttribute]
        [Display(Name = "* Article:")]
        public string ArticleId { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "* Issue date:")]
        [DateAttribute(minDate: "01/01/2020")]
        public DateTime IssueDate { get; set; }

        //[[Already have accepted???]] Ckech and validate?
        public bool IsAccepted { get; set; }

        public bool IsValid { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        //check with attribtute fro all acceptable types!!! but just pdf will accept!
        public string Extension { get; set; }

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
