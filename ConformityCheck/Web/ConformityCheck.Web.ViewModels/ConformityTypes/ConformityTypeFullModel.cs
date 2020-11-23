namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityTypeFullModel : IMapFrom<ConformityType>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
