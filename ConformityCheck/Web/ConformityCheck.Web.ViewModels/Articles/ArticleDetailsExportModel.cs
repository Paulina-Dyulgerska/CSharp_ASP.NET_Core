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

        // gyrmi mi instanciqta za nullna Suppliers, ako ne kaja izrishno na AutoMapper-a kak da mapva ot Article kym tozi class!!!!
        // ne moga da ostavq samo ArticleEditModel da iznese mappvaneto, a trqbwa i tuk da go opisha, inache
        // mi hvyrlq null za value na suppliers!
        public virtual void CreateMappings(IProfileExpression configuration)
        {
            // TODO - tova vzimane na ConformityTypes i Conformities e izlishno da go
            // opisvam az tuk, ako ConformityTypeExportModel i ConformityExportModel imat
            // IMapFrom<ConformityTypes> i syotvetno IMapFrom<Conformities> - AutoMapper-a sam
            // shte se seti da napravi tozi mappe-vane, ne trqbwa az da gi pisha, ako e izpylneno
            // towa!
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
