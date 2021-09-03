namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SuppliersIndexModel : PagingViewModel, IMapFrom<Supplier>
    {
        public IEnumerable<SupplierExportModel> Suppliers { get; set; }

        // this is the choosen supplier id
        [Required]
        [SupplierEntityAttribute]
        public string Id { get; set; }

        public string NumberSortParam => this.CurrentSortOrder == GlobalConstants.NumberSortParamDesc ?
           GlobalConstants.NumberSortParam : GlobalConstants.NumberSortParamDesc;

        public string NameSortParam => this.CurrentSortOrder == GlobalConstants.NameSortParamDesc ?
           GlobalConstants.NameSortParam : GlobalConstants.NameSortParamDesc;

        public string ArticlesCountSortParam => this.CurrentSortOrder == GlobalConstants.ArticlesCountSortParamDesc ?
           GlobalConstants.ArticlesCountSortParam : GlobalConstants.ArticlesCountSortParamDesc;

        public string UserEmailSortParam => this.CurrentSortOrder == GlobalConstants.UserEmailSortParamDesc ?
           GlobalConstants.UserEmailSortParam : GlobalConstants.UserEmailSortParamDesc;
    }
}
