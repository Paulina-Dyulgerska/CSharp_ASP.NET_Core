namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ConformityTypeConfiguration : IEntityTypeConfiguration<ConformityType>
    {
        public void Configure(EntityTypeBuilder<ConformityType> conformityType)
        {
            // cannot delete ConformityType if there are Conformities for this ConformityType uploaded!
            conformityType
                .HasMany(ct => ct.Conformities)
                .WithOne(c => c.ConformityType)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
