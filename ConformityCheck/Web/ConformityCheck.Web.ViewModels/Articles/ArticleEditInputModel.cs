namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleEditInputModel : IMapFrom<Article>, IHaveCustomMappings
    {
        private IEnumerable<SupplierExportModel> suppliers;

        [ArticleEntityAttribute]
        public string Id { get; set; }

        public string Number { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        [Display(Name = "* Article description:")]
        //[DescriptionRegExAttribute]
        public string Description { get; set; }

        public IEnumerable<SupplierExportModel> Suppliers
        {
            get
            {
                return this.suppliers;
            }

            set
            {
                this.suppliers = value;

                foreach (var supplier in this.suppliers)
                {
                    supplier.HasAllConformed = true;

                    foreach (var conformityType in this.ConformityTypes)
                    {
                        var conformity = this.Conformities
                            .FirstOrDefault(x => x.ConformityType.Id == conformityType.Id
                                            && x.Supplier.Id == supplier.Id);
                        if (conformity == null || !conformity.IsAccepted || !conformity.IsValid)
                        {
                            supplier.HasAllConformed = false;
                            break;
                        }
                    }
                }
            }
        }

        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

        public IEnumerable<ConformityExportModel> Conformities { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleEditInputModel>()
                .ForMember(
                x => x.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)))
                .ForMember(
                x => x.ConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .OrderBy(x => x.Id)))
                .ForMember(
                x => x.Conformities,
                opt => opt.MapFrom(a => a.Conformities
                .OrderBy(x => x.Id)));
        }
    }
}
