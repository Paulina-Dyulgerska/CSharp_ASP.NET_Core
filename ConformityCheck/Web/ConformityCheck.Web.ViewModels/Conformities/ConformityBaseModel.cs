namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services;
    using ConformityCheck.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public abstract class ConformityBaseModel : IMapFrom<Conformity>, IHaveCustomMappings, IValidatableObject
    {
        [Required]
        [Display(Name = "* Conformity type:")]
        [ConformityTypeEntityAttribute]
        public int ConformityTypeId { get; set; }

        public string ConformityTypeDescription { get; set; }

        [Required]
        [Display(Name = "* Supplier:")]
        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierNumber { get; set; }

        [Display(Name = "* Article:")]
        [ArticleEntityAttribute(allowNull: true)]
        public string ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        public string ConformityFileUrl { get; set; }

        // vsichki dates da sa v UTC, i tuk i na servera i na DB-a!!!
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "* Issue date:")]
        [DateAttribute(minDate: "01/01/2000")]
        public DateTime IssueDate { get; set; }

        public bool IsAccepted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Validity date:")]
        public DateTime? ValidityDate { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }

        [Required]
        [FileExtensionAttribute(extension: "pdf")]
        [FileSizeAttribute(size: 10 * 1024 * 1024)]
        public IFormFile InputFile { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Conformity, ConformityBaseModel>()
                .ForMember(x => x.ConformityTypeDescription, opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(x => x.SupplierName, opt => opt.MapFrom(x => x.Supplier.Name))
                .ForMember(x => x.SupplierNumber, opt => opt.MapFrom(x => x.Supplier.Number))
                .ForMember(x => x.ArticleDescription, opt => opt.MapFrom(x => x.Article.Description))
                .ForMember(x => x.ArticleNumber, opt => opt.MapFrom(x => x.Article.Number))
                .ForMember(x => x.ConformityFileUrl, opt =>
                    opt.MapFrom(x =>
                        x.ConformityFile.RemoteConformityFileUrl != null ?
                        x.ConformityFile.RemoteConformityFileUrl :
                        "/conformityFiles/conformities/" + x.ConformityFile.Id + "." + x.ConformityFile.Extension));
            // /wwwroot/conformityFiles/conformities/jhdsi-343g3h453-=g34g.pdf
        }

        public virtual IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if ((this.IssueDate > this.ValidityDate) && (this.ValidityDate != null))
            {
                yield return new ValidationResult("Issue date could not be after the validity date.");
            }

            var context = (IContentCheckService)validationContext
                .GetService(typeof(IContentCheckService));
            var articleSupplierEntity = context
                .ArticleSupplierEntityIdCheck(this.ArticleId, this.SupplierId);

            if (!articleSupplierEntity && this.ArticleId != null)
            {
                yield return new ValidationResult("The article does not have such supplier.");
            }
        }
    }
}
