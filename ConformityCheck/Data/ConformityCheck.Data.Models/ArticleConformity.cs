namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ArticleConformity : BaseModel<int>
    {
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
