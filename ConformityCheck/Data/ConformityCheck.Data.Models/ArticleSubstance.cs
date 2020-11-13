namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ArticleSubstance : BaseModel<int>
    {
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Substance))]
        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }
    }
}