namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> supplier)
        {
            // cannot delete Supplier if there are Conformities for this Supplier uploaded!
            supplier
                .HasMany(s => s.Conformities)
                .WithOne(a => a.Supplier)
                .OnDelete(DeleteBehavior.Restrict);

            supplier
                .HasIndex(p => p.Number)
                .IsUnique();
        }
    }
}
