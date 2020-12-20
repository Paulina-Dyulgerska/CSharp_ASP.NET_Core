namespace ConformityCheck.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ConformityFile : BaseDeletableModel<string>
    {
        public ConformityFile()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(Conformity))]
        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }

        public string Extension { get; set; }

        // The content of the files is in the file system. What about the expernal etorages?
        // If there is a file stored in the local system, I will find it by name, but if the file
        // is stoored on another external storage system, I will use the FileUrl as an locator.
        public string RemoteConformityFileUrl { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
