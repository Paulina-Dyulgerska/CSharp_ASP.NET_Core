﻿namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ConformityModel : IMapFrom<Conformity>
    {
        // [ConformityEntityAttribute]
        public string Id { get; set; }

        public ConformityTypeModel ConformityType { get; set; }

        public SupplierModel Supplier { get; set; }

        public ArticleBaseModel Article { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsValid => this.IsAccepted && this.ValidityDate >= DateTime.UtcNow.Date;

        public DateTime IssueDate { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        public string FileUrl { get; set; }

        public string Extension { get; set; }
    }
}