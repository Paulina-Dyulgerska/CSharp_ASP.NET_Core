namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Products;
    using ConformityCheck.Web.ViewModels.Substances;
    using ConformityCheck.Web.ViewModels.Suppliers;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateArticleInputModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        [Display(Name = "Article Nr.")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        [Display(Name = "Article description")]
        public string Description { get; set; }

        public SupplierInputModel Supplier { get; set; }

        [Display(Name = "Select conformity types")]
        public IEnumerable<int> ConformityTypes { get; set; }

        [Display(Name = "Select assembly to include article in")]
        public IEnumerable<ProductNumberExportModel> ProductsAvailable { get; set; } =
            new List<ProductNumberExportModel>();

        [Display(Name = "Select substances in article")]
        public IEnumerable<SubstanceNumberExportModel> SubstancesAvailable { get; set; } =
            new List<SubstanceNumberExportModel>();

        public IEnumerable<SupplierExportModel> SuppliersAvailable { get; set; } =
            new List<SupplierExportModel>();

        public IEnumerable<ConformityTypeNumberModel> ConformityTypesAvailable { get; set; } =
            new List<ConformityTypeNumberModel>();
    }
}
