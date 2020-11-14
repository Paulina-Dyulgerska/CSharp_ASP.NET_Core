namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleConformityTypeConfiguration : IEntityTypeConfiguration<ArticleConformityType>
    {
        public void Configure(EntityTypeBuilder<ArticleConformityType> articleConformityType)
        {
            //articleConformityType
            //     .HasKey(x => new { x.ArticleId, x.ConformityTypeId });

            articleConformityType
                .HasOne(ac => ac.Article)
                .WithMany(a => a.ArticleConformityTypes)
                .HasForeignKey(c => c.ArticleId);

            articleConformityType
                .HasOne(ac => ac.ConformityType)
                .WithMany(c => c.ArticleConformityTypes)
                .HasForeignKey(a => a.ConformityTypeId);
        }
    }
}
