namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleProduct
    {
        [ForeignKey(nameof(Article))]
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
