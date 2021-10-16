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

    public abstract class ConformityBaseModel : IMapFrom<Conformity>, IHaveCustomMappings, IValidatableObject
    {
        private DateTime issueDate;

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
        [ArticleEntityAttribute(allowNull: false)]
        public string ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleDescription { get; set; }

        // TODO - All dates are in UTC in the BE and DB
        // for FE - ToStringFOrmat(), but only in the Views; for fields - depending on the users local settings
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "* Issue date:")]
        [DateAttribute(minDate: "01/01/2000")]
        public DateTime IssueDate
        {
            get { return this.issueDate != DateTime.Parse("01/01/0001") ? this.issueDate : DateTime.UtcNow.Date; }
            set { this.issueDate = value; }
        }

        public bool IsAccepted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Validity date:")]
        public DateTime? ValidityDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public string Comments { get; set; }

        public string CallerViewName { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Conformity, ConformityBaseModel>()

                // It is not needed to explicitly map them here, because AutoMapper maps them
                // automatically through the navigation property of the Conformity class object!
                // They are kept because I am learning how to code now and to not forget this!
                .ForMember(x => x.ConformityTypeDescription, opt => opt.MapFrom(x => x.ConformityType.Description))
                .ForMember(x => x.SupplierName, opt => opt.MapFrom(x => x.Supplier.Name))
                .ForMember(x => x.SupplierNumber, opt => opt.MapFrom(x => x.Supplier.Number))
                .ForMember(x => x.ArticleDescription, opt => opt.MapFrom(x => x.Article.Description))
                .ForMember(x => x.ArticleNumber, opt => opt.MapFrom(x => x.Article.Number));
        }

        public virtual IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));
            var articleSupplierEntity = context.ArticleSupplierEntityIdCheck(this.ArticleId, this.SupplierId);

            if (!articleSupplierEntity && this.ArticleId != null)
            {
                yield return new ValidationResult("The article does not have such supplier.");
            }

            if ((this.IssueDate > this.ValidityDate) && (this.ValidityDate != null))
            {
                yield return new ValidationResult("Issue date could not be after the validity date.");
            }
        }
    }
}
