namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityValidityExportModel : IMapFrom<Conformity>, IMapFrom<ConformityExportModel>
    {
        public string Id { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsValid => this.IsAccepted && this.ValidityDate > DateTime.UtcNow.Date;
    }
}
