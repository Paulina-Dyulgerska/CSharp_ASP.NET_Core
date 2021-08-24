namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.Json.Serialization;

    using AutoMapper;
    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;
    using ConformityCheck.Web.ViewModels.Conformities;

    public class SupplierArticlesDetailsExportModel : PagingViewModel, IMapFrom<Supplier>, IHaveCustomMappings
    {
        private IEnumerable<ConformityExportModel> conformities;

        public string Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPersonName { get; set; }

        public string UserEmail { get; set; }

        public string NumberSortParam => string.IsNullOrEmpty(this.CurrentSortOrder) ?
            GlobalConstants.NumberSortParamDesc : string.Empty;

        public string DescriptionSortParam => this.CurrentSortOrder == GlobalConstants.DescriptionSortParamDesc ?
            GlobalConstants.DescriptionSortParam : GlobalConstants.DescriptionSortParamDesc;

        public string ConformityTypeSortParam => this.CurrentSortOrder == GlobalConstants.ConformityTypeSortParamDesc ?
            GlobalConstants.ConformityTypeSortParam : GlobalConstants.ConformityTypeSortParamDesc;

        public string HasConformitySortParam => this.CurrentSortOrder == GlobalConstants.HasConformitySortParamDesc ?
             GlobalConstants.HasConformitySortParam : GlobalConstants.HasConformitySortParamDesc;

        public string AcceptedConformitySortParam => this.CurrentSortOrder == GlobalConstants.AcceptedConformitySortParamDesc ?
             GlobalConstants.AcceptedConformitySortParam : GlobalConstants.AcceptedConformitySortParamDesc;

        public string ValidConformitySortParam => this.CurrentSortOrder == GlobalConstants.ValidConformitySortParamDesc ?
            GlobalConstants.ValidConformitySortParam : GlobalConstants.ValidConformitySortParamDesc;

        [Display(Name = "Articles:")]
        [JsonInclude]
        public IEnumerable<ArticleConformityExportModel> Articles { get; set; }

        [JsonInclude]
        public IEnumerable<ConformityExportModel> Conformities
        {
            get
            {
                return this.conformities;
            }

            private set
            {
                this.conformities = value;

                foreach (var article in this.Articles)
                {
                    article.ArticleConformity = this.conformities
                        .AsQueryable()
                        .Where(c => c.Article.Id == article.ArticleId &&
                                    c.ConformityType.Id == article.ConformityTypeId)

                        // .Select(c => new ConformityValidityExportModel
                        // {
                        //     Id = c.Id,
                        //     IsAccepted = c.IsAccepted,
                        //     ValidityDate = c.ValidityDate,
                        // })
                        .To<ConformityValidityExportModel>()
                        .FirstOrDefault();
                }
            }
        }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, SupplierArticlesDetailsExportModel>()
                .ForMember(x => x.Articles, opt => opt.MapFrom(s => s.ArticleSuppliers
                                                         .OrderByDescending(s => s.Article.Number)
                                                         .SelectMany(a => a.Article.ArticleConformityTypes)))
                .ForMember(x => x.ContactPersonName, opt =>
                              opt.MapFrom(s => $"{s.ContactPersonFirstName} {s.ContactPersonLastName}"))
                .ForMember(x => x.ItemsCount, opt => opt.MapFrom(s => s.ArticleSuppliers
                                                           .SelectMany(a => a.Article.ArticleConformityTypes).Count()));
        }
    }
}
