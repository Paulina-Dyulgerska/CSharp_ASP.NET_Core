namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> article)
        {
            // cannot delete Article if there are Conformities for this Article uploaded!
            article
                .HasMany(a => a.Conformities)
                .WithOne(c => c.Article)
                .OnDelete(DeleteBehavior.Restrict);

            article
                .HasIndex(a => a.Number)
                .IsUnique();
        }
    }
}
