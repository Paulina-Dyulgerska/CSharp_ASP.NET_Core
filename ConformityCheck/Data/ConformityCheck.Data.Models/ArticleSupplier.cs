namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ArticleSupplier : BaseModel<int>
    {
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public string SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public bool IsMainSupplier { get; set; }
    }
}
