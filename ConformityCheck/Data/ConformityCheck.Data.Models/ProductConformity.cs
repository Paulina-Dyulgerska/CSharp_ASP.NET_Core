namespace ConformityCheck.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using ConformityCheck.Data.Common.Models;

    public class ProductConformity : BaseModel<int>
    {
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey(nameof(Conformity))]
        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
