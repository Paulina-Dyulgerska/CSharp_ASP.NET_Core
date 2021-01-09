namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticlesListAllModel : PagingViewModel, IMapFrom<Article>
    {
        public IEnumerable<ArticleDetailsModel> Articles { get; set; }

        public ArticleListAllInputModel ListAllInputModel { get; set; }
    }
}
