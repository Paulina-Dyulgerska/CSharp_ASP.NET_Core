namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleManageSuppliersModel : ArticleBaseModel, IHaveCustomMappings
    {
        [ArticleEntityAttribute]
        public string Id { get; set; }

        // Required e here to activate the Required in the client-side validation.
        [Required]
        public string SupplierId { get; set; }

        public IEnumerable<SupplierExportModel> Suppliers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleManageSuppliersModel>()
                .ForMember(x => x.Suppliers, opt => opt.MapFrom(a => a.ArticleSuppliers
                                                         .OrderByDescending(x => x.IsMainSupplier)
                                                         .ThenBy(x => x.Supplier.Name)));
        }
    }
}
