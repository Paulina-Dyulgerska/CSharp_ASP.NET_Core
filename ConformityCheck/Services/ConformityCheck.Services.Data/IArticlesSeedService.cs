namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface IArticlesSeedService
    {
        Task CreateAsync(ArticleImportDTO articleImportDTO);
    }
}
