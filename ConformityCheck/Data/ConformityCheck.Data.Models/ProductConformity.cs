namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductConformity
    {
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey(nameof(Conformity))]
        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
