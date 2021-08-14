namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleProductConfiguration : IEntityTypeConfiguration<ArticleProduct>
    {
        public void Configure(EntityTypeBuilder<ArticleProduct> articleProduct)
        {
            articleProduct
                .HasOne(ap => ap.Article)
                .WithMany(p => p.ArticleProducts)
                .HasForeignKey(ap => ap.ArticleId);

            articleProduct
                .HasOne(ap => ap.Product)
                .WithMany(a => a.ArticleProducts)
                .HasForeignKey(ap => ap.ProductId);
        }
    }
}
