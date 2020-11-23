namespace ConformityCheck.Web.ViewModels.Conformities
{
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using System;

    public class ConformityModel : IMapFrom<Conformity>
    {
        public string Id { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        public DateTime? IssueDate { get; set; }

        public DateTime? ConformationAcceptanceDate { get; set; }

        public bool IsAccepted { get; set; }

    }
}
