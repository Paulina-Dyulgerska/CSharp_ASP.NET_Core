namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ArticleSupplier : BaseModel<int>
    {
        [ForeignKey(nameof(Article))]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [ForeignKey(nameof(Supplier))]
        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
