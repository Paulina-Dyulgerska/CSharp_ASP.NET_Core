namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConformityConfiguration : IEntityTypeConfiguration<ProductConformity>
    {
        public void Configure(EntityTypeBuilder<ProductConformity> productConformity)
        {
            productConformity.HasKey(x => new { x.ProductId, x.ConformityId });

            productConformity.HasOne(pc => pc.Product)
            .WithMany(p => p.ProductConformities)
            .HasForeignKey(c => c.ProductId);

            productConformity.HasOne(pc => pc.Conformity)
            .WithMany(c => c.ProductConformities)
            .HasForeignKey(p => p.ConformityId);
        }
    }
}
