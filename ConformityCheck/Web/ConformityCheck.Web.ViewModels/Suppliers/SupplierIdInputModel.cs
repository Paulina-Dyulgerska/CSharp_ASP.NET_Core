namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierIdInputModel
    {
        [SupplierEntityAttribute]
        public string Id { get; set; }
    }
}
