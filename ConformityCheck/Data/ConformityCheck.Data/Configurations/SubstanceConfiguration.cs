namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SubstanceConfiguration : IEntityTypeConfiguration<Substance>
    {
        public void Configure(EntityTypeBuilder<Substance> substance)
        {
            substance
                .HasIndex(s => s.CASNumber)
                .IsUnique();
        }
    }
}
