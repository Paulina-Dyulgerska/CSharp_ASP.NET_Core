namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ArticleProduct : BaseModel<int>
    {
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
