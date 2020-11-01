namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleConformity
    {
        [ForeignKey(nameof(Article))] //moga da gi iztriq vsichki takiwa
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Conformity))]
        public int ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
