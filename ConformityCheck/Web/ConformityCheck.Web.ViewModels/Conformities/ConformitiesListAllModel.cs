namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System.Collections.Generic;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformitiesListAllModel : PagingViewModel, IMapFrom<Conformity>
    {
        public IEnumerable<ConformityExportModel> Conformities { get; set; }

        [ConformityEntityAttribute]
        public string Id { get; set; }

        public string ArticleNumberSortParam { get; set; }

        public string ArticleDescriptionSortParam { get; set; }

        public string SupplierNumberSortParam { get; set; }

        public string SupplierNameSortParam { get; set; }

        public string ConformityTypeDescriptionSortParam { get; set; }

        public string IsAcceptedSortParam { get; set; }

        public string IsValidSortParam { get; set; }

        public string UserEmailSortParm { get; set; }

        // public string ModifiedOnSortParm { get; set; }
    }
}
