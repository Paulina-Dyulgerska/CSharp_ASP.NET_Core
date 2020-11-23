using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConformityCheck.Web.ViewModels.Conformities
{
   public class ConformityCreateModel
    {
        [Required]
        public string ConformityType { get; set; }

        [Required]
        public string ArticleNumber { get; set; }

        [Required]
        public string SupplierNumber { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        public DateTime IssueDate { get; set; }

        public DateTime? ConformationAcceptanceDate { get; set; }

        public bool IsAssepted { get; set; }
    }
}
