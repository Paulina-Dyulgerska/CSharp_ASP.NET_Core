namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System.Collections.Generic;

    using ConformityCheck.Common;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformitiesIndexModel : PagingViewModel, IMapFrom<Conformity>
    {
        public IEnumerable<ConformityExportModel> Conformities { get; set; }

        [ConformityEntityAttribute]
        public string Id { get; set; }

        public string ArticleNumberSortParam => this.CurrentSortOrder == GlobalConstants.ArticleNumberSortParamDesc ?
            GlobalConstants.ArticleNumberSortParam : GlobalConstants.ArticleNumberSortParamDesc;

        public string ArticleDescriptionSortParam => this.CurrentSortOrder == GlobalConstants.ArticleDescriptionSortParamDesc ?
            GlobalConstants.ArticleDescriptionSortParam : GlobalConstants.ArticleDescriptionSortParamDesc;

        public string SupplierNumberSortParam => this.CurrentSortOrder == GlobalConstants.SupplierNumberSortParamDesc ?
            GlobalConstants.SupplierNumberSortParam : GlobalConstants.SupplierNumberSortParamDesc;

        public string SupplierNameSortParam => this.CurrentSortOrder == GlobalConstants.SupplierNameSortParamDesc ?
            GlobalConstants.SupplierNameSortParam : GlobalConstants.SupplierNameSortParamDesc;

        public string ConformityTypeDescriptionSortParam => this.CurrentSortOrder ==
            GlobalConstants.ConformityTypeDescriptionSortParamDesc ?
            GlobalConstants.ConformityTypeDescriptionSortParam : GlobalConstants.ConformityTypeDescriptionSortParamDesc;

        public string IsAcceptedSortParam => this.CurrentSortOrder == GlobalConstants.IsAcceptedSortParamDesc ?
            GlobalConstants.IsAcceptedSortParam : GlobalConstants.IsAcceptedSortParamDesc;

        public string IsValidSortParam => this.CurrentSortOrder == GlobalConstants.IsValidSortParamDesc ?
            GlobalConstants.IsValidSortParam : GlobalConstants.IsValidSortParamDesc;

        public string UserEmailSortParam => this.CurrentSortOrder == GlobalConstants.UserEmailSortParamDesc ?
            GlobalConstants.UserEmailSortParam : GlobalConstants.UserEmailSortParamDesc;

        // public string ModifiedOnSortParam { get; set; }
    }
}
