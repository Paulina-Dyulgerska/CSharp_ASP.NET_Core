namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ConformityConfiguration : IEntityTypeConfiguration<Conformity>
    {
        public void Configure(EntityTypeBuilder<Conformity> conformity)
        {
        }
    }
}
