namespace ConformityCheck.Data.Models
{
    using ConformityCheck.Data.Common.Models;

    public class ProductConformityType : BaseModel<int>
    {
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int ConformityTypeId { get; set; }

        public virtual ConformityType ConformityType { get; set; }
    }
}
