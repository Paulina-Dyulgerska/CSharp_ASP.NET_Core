namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;

    public class ArticleManageConformityTypesExportModel : ArticleBaseModel, IHaveCustomMappings
    {
        private IEnumerable<ConformityTypeExportModel> conformityTypes;

        public string Id { get; set; }

        public int ConformityTypeId { get; set; }

        public IEnumerable<ConformityTypeExportModel> ConformityTypes
        {
            get
            {
                return this.conformityTypes;
            }

            private set
            {
                this.conformityTypes = value;

                foreach (var conformityType in this.conformityTypes)
                {
                    conformityType.SupplierConfirmed = false;

                    foreach (var conformity in this.Conformities)
                    {
                        if (conformity.ConformityType.Id == conformityType.Id && conformity.IsAccepted && conformity.IsValid)
                        {
                            conformityType.SupplierConfirmed = true;
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<ConformityExportModel> Conformities { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageConformityTypesExportModel>()
                 .ForMember(x => x.ConformityTypes, opt => opt.MapFrom(a => a.ArticleConformityTypes
                                                     .OrderBy(x => x.ConformityTypeId)))
                 .ForMember(x => x.Conformities, opt => opt.MapFrom(a => a.Conformities))
                 ;
        }
    }
}
