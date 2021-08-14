namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleSubstanceConfiguration : IEntityTypeConfiguration<ArticleSubstance>
    {
        public void Configure(EntityTypeBuilder<ArticleSubstance> articleSubstance)
        {
            articleSubstance.HasOne(asub => asub.Article)
            .WithMany(a => a.ArticleSubstances)
            .HasForeignKey(s => s.ArticleId);

            articleSubstance.HasOne(asub => asub.Substance)
            .WithMany(s => s.ArticleSubstances)
            .HasForeignKey(a => a.SubstanceId);

            articleSubstance
                .HasQueryFilter(asub => !asub.Article.IsDeleted && !asub.Substance.IsDeleted);
        }
    }
}
