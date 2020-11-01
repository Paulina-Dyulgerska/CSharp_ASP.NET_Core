namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleSupplier
    {
        [ForeignKey(nameof(Article))]
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
