using System;
using System.Collections.Generic;
using System.Text;

namespace ConformityCheck.Web.ViewModels.Articles.Conformities
{
   public class ConformityExportModel
    {
        public int ConformityTypeId { get; set; }

        public string SupplierId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ConformationAcceptanceDate { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsValid { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }
    }
}
