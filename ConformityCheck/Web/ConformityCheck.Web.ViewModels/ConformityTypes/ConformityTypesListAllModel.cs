namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System.Collections.Generic;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypesListAllModel : PagingViewModel, IMapFrom<ConformityType>
    {
        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

        [ConformityEntityAttribute]
        public string Id { get; set; }

        public string IdSortParm { get; set; }

        public string DescriptionSortParm { get; set; }

        public string UserEmailSortParm { get; set; }

        public string ModifiedOnSortParm { get; set; }
    }
}
