namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SuppliersListAllModel : PagingViewModel
    {
        public IEnumerable<SupplierFullInfoModel> Suppliers { get; set; }
    }
}
