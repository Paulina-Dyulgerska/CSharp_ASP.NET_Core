namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticlesListAllModel : PagingViewModel, IMapFrom<Article>
    {
        public IEnumerable<ArticleDetailsModel> Articles { get; set; }

        [ArticleEntityAttribute]
        public string Id { get; set; }

        public string NumberSortParm { get; set; }

        public string DescriptionSortParm { get; set; }

        public string MainSupplierNumberSortParm { get; set; }

        public string MainSupplierNameSortParm { get; set; }

        public string MainSupplierAllConfirmedSortParm { get; set; }

        public string AllSuppliersAllConfirmedSortParm { get; set; }
    }
}
