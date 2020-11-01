namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleConformityConfiguration : IEntityTypeConfiguration<ArticleConformity>
    {
        public void Configure(EntityTypeBuilder<ArticleConformity> articleConformity)
        {
            articleConformity
                 .HasKey(x => new { x.ArticleId, x.ConformityId });

            articleConformity
                .HasOne(ac => ac.Article)
                .WithMany(a => a.ArticleConformities)
                .HasForeignKey(c => c.ArticleId);

            articleConformity
                .HasOne(ac => ac.Conformity)
                .WithMany(c => c.ArticleConformities)
                .HasForeignKey(a => a.ConformityId);
        }
    }
}
