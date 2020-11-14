namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Products;
    using ConformityCheck.Web.ViewModels.Substances;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class CreateArticleInputModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        public string Description { get; set; }

        //[RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9-]*")]
        //public string SupplierNumber { get; set; }

        //[RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 -_]*")]
        //public string SupplierName { get; set; }

        //[EmailAddress]
        //public string SupplierEmail { get; set; }

        //[RegularExpression("[0-9-+ ]")]
        //public string SupplierPhoneNumber { get; set; }

        //[RegularExpression("[A-Za-z]")]
        //public string ContactPersonFirstName { get; set; }

        //[RegularExpression("[A-Za-z]")]
        //public string ContactPersonLastName { get; set; }

        //public string UserId { get; set; }

        public ArticleConformityInputModel Conformity { get; set; }

        public IEnumerable<SupplierNumberExportModel> Suppliers { get; set; } =
            new List<SupplierNumberExportModel>();

        public IEnumerable<ConformityTypeNumberExportModel> ConformityTypes { get; set; } =
            new List<ConformityTypeNumberExportModel>();

        public IEnumerable<ProductNumberExportModel> Products { get; set; } =
            new List<ProductNumberExportModel>();

        public IEnumerable<SubstanceNumberExportModel> Substances { get; set; } =
            new List<SubstanceNumberExportModel>();
    }
}
