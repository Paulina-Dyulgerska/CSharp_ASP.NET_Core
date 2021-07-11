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

        public string NumberSortParm { get; set; }

        public string NameSortParm { get; set; }

        public string ArticlesCountSortParm { get; set; }

        public string UserEmailSortParm { get; set; }
    }
}
