namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ConformityCreateModel
    {
        [Required]
        public string ConformityTypeId { get; set; }

        [Required]
        public string SupplierId { get; set; }

        public string ArticleId { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        [Required]
        public DateTime IssueDate { get; set; }

        public bool IsAssepted { get; set; }

        public bool IsValid { get; set; }

        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }
    }
}
