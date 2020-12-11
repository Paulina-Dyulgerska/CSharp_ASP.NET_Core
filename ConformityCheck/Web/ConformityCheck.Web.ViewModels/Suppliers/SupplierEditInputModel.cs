namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierEditInputModel : SupplierBaseModel
    {
        [SupplierEntityAttribute]
        public string Id { get; set; }
    }
}
