namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;

    public class ArticleCreateInputModel : ArticleBaseInputModel
    {
        [SupplierEntityAttribute]
        public string SupplierId { get; set; }

        [Required]
        [Display(Name = "* Select conformity type/s")]
        // The validation of the Id of the choosen entities will be in the service!
        public IEnumerable<int> ConformityTypes { get; set; }

        public IEnumerable<string> Products { get; set; }

        public IEnumerable<string> Substances { get; set; }
    }
}
