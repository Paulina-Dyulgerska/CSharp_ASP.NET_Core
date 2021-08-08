namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ConformityExportModel : IMapFrom<Conformity>, IMapFrom<ConformityExportModel>, IHaveCustomMappings
    {
        [ConformityEntityAttribute]
        public string Id { get; set; }

        public ArticleExportModel Article { get; set; }

        public ConformityTypeExportModel ConformityType { get; set; }

        public SupplierExportModel Supplier { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsValid => this.IsAccepted && this.ValidityDate > DateTime.UtcNow.Date;

        public DateTime IssueDate { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public string Comments { get; set; }

        public string UserEmail { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ConformityFileUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            // /wwwroot/conformityFiles/conformities/jhdsi-343g3h453-=g34g.pdf
            configuration.CreateMap<Conformity, ConformityExportModel>()
                .ForMember(x => x.ConformityFileUrl, opt =>
                    opt.MapFrom(x =>
                        x.RemoteFileUrl != null ?
                        x.RemoteFileUrl :
                        "/files/conformities/" + x.Id + "." + x.FileExtension));
        }
    }
}
