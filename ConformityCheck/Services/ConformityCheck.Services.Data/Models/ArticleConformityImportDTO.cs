using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConformityCheck.Services.Models
{
   public class ArticleConformityImportDTO
    {
        [Required]
        public string ConformityType { get; set; }

        public DateTime IssueDate { get; set; } //vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!

        public DateTime? ConformationAcceptanceDate { get; set; }

        public bool IsAssepted { get; set; }

        public string Comments { get; set; }
        //the file of the conformity itseld? should be stored in the DB.
    }
}
