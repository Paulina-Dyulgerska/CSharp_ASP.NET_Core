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

    public class ArticleEditModel : ArticleBaseModel, /*IMapFrom<Article>, */IHaveCustomMappings
    {
        private IEnumerable<SupplierModel> suppliers;

        public ArticleEditModel()
        {
        }

        [ArticleEntityAttribute]
        public string Id { get; set; }

        public IEnumerable<SupplierModel> Suppliers
        {
            get
            {
                return this.suppliers;
            }

            set
            {
                this.suppliers = value;

                foreach (var item in this.suppliers)
                {
                    item.HasAllConformed = true;

                    foreach (var conformityType in this.ConformityTypes)
                    {
                        var conformity = this.Conformities
                            .FirstOrDefault(x => x.ConformityType.Id == conformityType.Id
                                            && x.Supplier.Id == item.Id);
                        if (conformity == null || !conformity.IsAccepted || !conformity.IsValid)
                        {
                            item.HasAllConformed = false;
                            break;
                        }
                    }
                }
            }
        }

        //public bool IsConfirmed => this.Suppliers.Count() > 0 && this.Suppliers.All(x => x.HasAllConformed);

        public IEnumerable<ConformityTypeModel> ConformityTypes { get; set; }

        public IEnumerable<ConformityModel> Conformities { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleEditModel>()
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
