namespace ConformityCheck.Web.ViewModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization; // using Newtonsoft.Json;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleDetailsExportModel : IMapFrom<Article>, IHaveCustomMappings
    {
        private IEnumerable<SupplierExportModel> suppliers;

        public string Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public bool IsConfirmed => this.Suppliers.Count() > 0 && this.Suppliers.All(x => x.HasAllConformed);

        public bool IsConfirmedByMainSupplier => this.Suppliers.Any(x => x.IsMainSupplier && x.HasAllConformed);

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserEmail { get; set; }

        // [JsonProperty]
        [JsonInclude]
        public IEnumerable<ConformityTypeExportModel> ConformityTypes { get; set; }

        // [JsonProperty]
        [JsonInclude]
        public IEnumerable<ConformityExportModel> Conformities { get; set; }

        // [JsonProperty]
        [JsonInclude]
        public IEnumerable<SupplierExportModel> Suppliers
        {
            get
            {
                return this.suppliers;
            }

            private set
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

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }

        // the instance created throws exception by null for Suppliers, if i do not tell explicitly to the AutoMapper
        // how to map from Article to this class!
        // Cannot just leave just ArticleEditModel to carry out the mapping, but I have to describe the mapping here too
        // if I do not want thrown exception by null for the Suppliers value!
        public virtual void CreateMappings(IProfileExpression configuration)
        {
            // TODO
            // It is redundant to make ConformityTypes and Conformities here and to describe it here IF
            // the ConformityTypeExportModel and the ConformityExportModel classes have
            // IMapFrom<ConformityTypes>, respectfully: IMapFrom<Conformities>. If they have it, then AutoMapper
            // will do this mapping alone - without me telling this to AutoMapper!
            configuration.CreateMap<Article, ArticleDetailsExportModel>()
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

                // .ForMember(
                // x => x.ArticleConformityTypes,
                // opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                // $"{x.ConformityType.Description} => {x.Conformity.IsAccepted}").ToList()))
                // .ForMember(
                // x => x.ArticleMissingConformityTypes,
                // opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                // $"{x.ConformityType.Description} => {x.Conformity != null}").ToList()))
                ;
        }
    }
}
