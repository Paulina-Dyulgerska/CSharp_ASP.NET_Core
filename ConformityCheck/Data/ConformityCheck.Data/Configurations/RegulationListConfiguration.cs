namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RegulationListConfiguration : IEntityTypeConfiguration<RegulationList>
    {
        public void Configure(EntityTypeBuilder<RegulationList> regulationList)
        {
            regulationList
                .HasIndex(rl => rl.Description)
                .IsUnique();
        }
    }
}
