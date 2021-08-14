namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SubstanceRegulationListConfiguration : IEntityTypeConfiguration<SubstanceRegulationList>
    {
        public void Configure(EntityTypeBuilder<SubstanceRegulationList> substanceRegulationList)
        {
            substanceRegulationList.HasOne(srl => srl.RegulationList)
            .WithMany(rl => rl.SubstanceRegulationLists)
            .HasForeignKey(s => s.RegulationListId);

            substanceRegulationList.HasOne(srl => srl.Substance)
            .WithMany(s => s.SubstanceRegulationLists)
            .HasForeignKey(rl => rl.SubstanceId);
        }
    }
}
