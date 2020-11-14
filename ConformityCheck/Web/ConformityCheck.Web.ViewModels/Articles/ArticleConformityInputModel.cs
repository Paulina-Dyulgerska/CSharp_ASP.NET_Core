namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ArticleConformityInputModel
    {
        public string Id { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        public DateTime IssueDate { get; set; }

        public DateTime? ConformationAcceptanceDate { get; set; }

        public bool IsAssepted { get; set; }
    }
}
