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
    }
}
