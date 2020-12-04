namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeFullInfoModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
