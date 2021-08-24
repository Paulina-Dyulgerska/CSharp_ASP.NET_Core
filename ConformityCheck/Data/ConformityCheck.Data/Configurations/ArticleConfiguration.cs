namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> article)
        {
            article
                .HasIndex(a => a.Number)
                .IsUnique();
        }
    }
}
