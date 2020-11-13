namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ArticleConformity : BaseModel<int>
    {
        // moga da gi iztriq vsichki takiwa attributes, nenujni sa
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Conformity))]
        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
