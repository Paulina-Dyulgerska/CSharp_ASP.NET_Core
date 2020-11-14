using System.ComponentModel.DataAnnotations;

namespace ConformityCheck.Web.ViewModels.Suppliers
{
    public class SupplierInputModel
    {
        [Display(Name = "Supplier")]
        public string Id { get; set; }
    }
}
