namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleSubstance
    {
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Substance))]
        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }
    }
}