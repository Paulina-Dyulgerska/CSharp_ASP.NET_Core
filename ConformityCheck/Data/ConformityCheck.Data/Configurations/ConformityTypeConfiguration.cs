﻿namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ConformityTypeConfiguration : IEntityTypeConfiguration<ConformityType>
    {
        public void Configure(EntityTypeBuilder<ConformityType> conformityType)
        {
            conformityType.HasMany(ct => ct.Conformities)
            .WithOne(c => c.ConformityType)
            .OnDelete(DeleteBehavior.Restrict); //ne moje da se iztrie ConformityType, predi
                                                //da se iztriqt vsichki Conformities, koito sa ot tozi type!

            conformityType.HasIndex(ct => ct.Description)
            .IsUnique();
        }
    }
}
