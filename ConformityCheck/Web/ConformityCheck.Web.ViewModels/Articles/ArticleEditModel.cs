namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleEditModel : ArticleModel, IHaveCustomMappings
    {
        private IEnumerable<SupplierModel> suppliers;

        public bool IsConfirmed => this.Suppliers.All(x => x.HasAllConformed);

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
                    foreach (var conformityType in this.ConformityTypes)
                    {
                        var conformity = this.Conformities
                            .FirstOrDefault(x => x.ConformityType.Id == conformityType.Id
                                            && x.Supplier.Id == item.Id);
                        if (conformity != null && conformity.IsAccepted && conformity.IsValid)
                        {
                            item.HasAllConformed = true;
                        }
                    }
                }
            }
        }

        public IEnumerable<ConformityTypeModel> ConformityTypes { get; set; }

        public IEnumerable<ConformityExportModel> Conformities { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        //public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        //public IEnumerable<string> ArticleConformityTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
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
                .OrderBy(x => x.Id)))
                //.ForMember(
                //x => x.ArticleConformityTypes,
                //opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                //$"{x.ConformityType.Description} => {x.Conformity.IsAccepted}").ToList()))
                //.ForMember(
                //x => x.ArticleMissingConformityTypes,
                //opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                //$"{x.ConformityType.Description} => {x.Conformity != null}").ToList()))
                ;
        }
    }
}
