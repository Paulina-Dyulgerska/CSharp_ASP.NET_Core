namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleSupplierConfiguration : IEntityTypeConfiguration<ArticleSupplier>
    {
        public void Configure(EntityTypeBuilder<ArticleSupplier> articleSupplier)
        {
            articleSupplier.HasOne(asup => asup.Article)
            .WithMany(a => a.ArticleSuppliers)
            .HasForeignKey(s => s.ArticleId);

            articleSupplier.HasOne(asup => asup.Supplier)
            .WithMany(s => s.ArticleSuppliers)
            .HasForeignKey(a => a.SupplierId);
        }
    }
}
