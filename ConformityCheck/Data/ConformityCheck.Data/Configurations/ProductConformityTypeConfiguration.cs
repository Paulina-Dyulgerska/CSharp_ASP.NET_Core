namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConformityTypeConfiguration : IEntityTypeConfiguration<ProductConformityType>
    {
        public void Configure(EntityTypeBuilder<ProductConformityType> productConformityType)
        {
            // productConformity.HasKey(x => new { x.ProductId, x.ConformityId });
            productConformityType
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductConformityTypes)
                .HasForeignKey(c => c.ProductId);

            productConformityType
                .HasOne(pc => pc.ConformityType)
                .WithMany(c => c.ProductConformityTypes)
                .HasForeignKey(p => p.ConformityTypeId);

            productConformityType
                .HasQueryFilter(pc => !pc.Product.IsDeleted && !pc.ConformityType.IsDeleted);
        }
    }
}
