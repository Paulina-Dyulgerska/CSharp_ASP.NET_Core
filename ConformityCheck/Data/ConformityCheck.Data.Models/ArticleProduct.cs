namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ArticleProduct : BaseModel<int>
    {
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
