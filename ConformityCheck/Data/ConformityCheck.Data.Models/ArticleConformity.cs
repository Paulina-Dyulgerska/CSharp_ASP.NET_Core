namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleConformity
    {
        [ForeignKey(nameof(Article))] //moga da gi iztriq vsichki takiwa
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Conformity))]
        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
