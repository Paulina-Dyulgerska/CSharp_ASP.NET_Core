namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ConformityCreateModel
    {
        [Required]
        [Display(Name = "* Conformity type:")]
        public string ConformityTypeId { get; set; }

        [Required]
        [Display(Name = "* Supplier:")]
        public string SupplierId { get; set; }

        public bool ValidForAllArticles { get; set; }

        public bool ValidForSingleArticle { get; set; }

        public string ArticleId { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "* Conformity date:")]
        public DateTime IssueDate { get; set; } //vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!

        public bool IsAssepted { get; set; }

        public bool IsValid { get; set; }

        public DateTime? ValidityDate { get; set; } //vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }
    }
}
