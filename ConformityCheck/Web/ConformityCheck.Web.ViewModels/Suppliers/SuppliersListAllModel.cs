namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SuppliersListAllModel : PagingViewModel, IMapFrom<Supplier>
    {
        public IEnumerable<SupplierFullInfoModel> Suppliers { get; set; }

        [SupplierEntityAttribute]
        public string Id { get; set; }
    }
}
