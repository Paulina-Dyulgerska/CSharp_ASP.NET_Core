namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticlesIndexModel : PagingViewModel, IMapFrom<Article>
    {
        public IEnumerable<ArticleDetailsExportModel> Articles { get; set; }

        public string NumberSortParam => this.CurrentSortOrder == GlobalConstants.NumberSortParamDesc ?
            GlobalConstants.NumberSortParam : GlobalConstants.NumberSortParamDesc;

        public string DescriptionSortParam => this.CurrentSortOrder == GlobalConstants.DescriptionSortParamDesc ?
            GlobalConstants.DescriptionSortParam : GlobalConstants.DescriptionSortParamDesc;

        public string MainSupplierNumberSortParam => this.CurrentSortOrder == GlobalConstants.MainSupplierNumberSortParamDesc ?
            GlobalConstants.MainSupplierNumberSortParam : GlobalConstants.MainSupplierNumberSortParamDesc;

        public string MainSupplierNameSortParam => this.CurrentSortOrder == GlobalConstants.MainSupplierNameSortParamDesc ?
            GlobalConstants.MainSupplierNameSortParam : GlobalConstants.MainSupplierNameSortParamDesc;

        public string MainSupplierAllConfirmedSortParam => this.CurrentSortOrder == GlobalConstants.MainSupplierAllConfirmedSortParamDesc ?
            GlobalConstants.MainSupplierAllConfirmedSortParam : GlobalConstants.MainSupplierAllConfirmedSortParamDesc;

        public string AllSuppliersAllConfirmedSortParam => this.CurrentSortOrder == GlobalConstants.AllSuppliersAllConfirmedSortParamDesc ?
            GlobalConstants.AllSuppliersAllConfirmedSortParam : GlobalConstants.AllSuppliersAllConfirmedSortParamDesc;
    }
}
