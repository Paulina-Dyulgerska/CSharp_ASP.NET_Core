namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ProductConformity : BaseModel<int>
    {
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string ConformityId { get; set; }

        public virtual Conformity Conformity { get; set; }
    }
}
