namespace ConformityCheck.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class Conformity : BaseDeletableModel<string>
    {
        public Conformity()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(ConformityType))]
        public int ConformityTypeId { get; set; }

        public virtual ConformityType ConformityType { get; set; }

        [Required]
        [ForeignKey(nameof(Supplier))]
        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        [Required]
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        // [Required]
        // [ForeignKey(nameof(ConformityFile))]
        // public string ConformityFileId { get; set; }

        // public virtual ConformityFile ConformityFile { get; set; }

        // the file name is this.Id!
        public string FileExtension { get; set; }

        // The content of the files is in the file system. What about the expernal storages?
        // If there is a file stored in the local system, I will find it by name, but if the file
        // is stored on another external storage system, I will use the FileUrl as an locator.
        public string RemoteFileUrl { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
