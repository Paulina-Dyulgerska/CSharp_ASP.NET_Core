namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityValidityExportModel : IMapFrom<Conformity>
    {
        public string Id { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? ValidityDate { get; set; }
    }
}
