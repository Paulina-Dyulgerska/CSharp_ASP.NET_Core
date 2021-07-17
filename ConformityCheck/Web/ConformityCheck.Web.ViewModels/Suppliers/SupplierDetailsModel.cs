namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Articles;

    public class SupplierDetailsModel : SupplierEditInputModel
    {
        [Display(Name = "Articles:")]
        public IEnumerable<ArticleConformityExportModel> Articles { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Supplier, SupplierDetailsModel>()
        //        .ForMember(
        //        x => x.Articles,
        //        opt => opt.MapFrom(s => s.ArticleSuppliers
        //                                .SelectMany(x => x.Article.ArticleConformityTypes
        //                                                         .Select(ac => new ArticleConformityExportModel
        //                                                         {
        //                                                             ArticleConformityType = ac,
        //                                                             ArticleConformity = x.Article
        //                                                                             .Conformities
        //                                                                             .Where(
        //                                                                                c => c.SupplierId == x.SupplierId
        //                                                                             && c.ConformityTypeId == ac.ConformityTypeId
        //                                                                             )
        //                                                                             .FirstOrDefault(),
        //                                                         }))));
        //}
    }
}
