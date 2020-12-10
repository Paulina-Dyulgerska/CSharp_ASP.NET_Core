namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using ConformityCheck.Common.ValidationAttributes;

    public class SupplierEditModel : SupplierBaseModel
    {
        [SupplierEntityAttribute]
        public string Id { get; set; }

        //public IEnumerable<> Articles { get; set; }

        //public IEnumerable<> Conformities { get; set; }
    }
}
