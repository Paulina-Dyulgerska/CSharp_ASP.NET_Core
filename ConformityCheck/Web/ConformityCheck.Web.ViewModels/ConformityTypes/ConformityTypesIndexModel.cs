namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System.Collections.Generic;

    using ConformityCheck.Common;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypesIndexModel : PagingViewModel, IMapFrom<ConformityType>
    {
        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

        [ConformityEntityAttribute]
        public string Id { get; set; }

        public string IdSortParam => this.CurrentSortOrder == GlobalConstants.IdSortParamDesc ?
             GlobalConstants.IdSortParam : GlobalConstants.IdSortParamDesc;

        public string DescriptionSortParam => this.CurrentSortOrder == GlobalConstants.DescriptionSortParamDesc ?
             GlobalConstants.DescriptionSortParam : GlobalConstants.DescriptionSortParamDesc;

        public string UserEmailSortParam => this.CurrentSortOrder == GlobalConstants.UserEmailSortParamDesc ?
             GlobalConstants.UserEmailSortParam : GlobalConstants.UserEmailSortParamDesc;

        public string ModifiedOnSortParam => this.CurrentSortOrder == GlobalConstants.ModifiedOnSortParamDesc ?
             GlobalConstants.ModifiedOnSortParam : GlobalConstants.ModifiedOnSortParamDesc;
    }
}
