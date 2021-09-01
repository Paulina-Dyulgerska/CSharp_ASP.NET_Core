namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ArticleSubstance : BaseModel<int>
    {
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int SubstanceId { get; set; }

        public virtual Substance Substance { get; set; }
    }
}
