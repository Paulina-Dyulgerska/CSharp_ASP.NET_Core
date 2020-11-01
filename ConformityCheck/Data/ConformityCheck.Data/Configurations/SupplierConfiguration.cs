namespace ConformityCheck.Data.Configurations
{
    using ConformityCheck.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> supplier)
        {
            supplier
                .HasMany(s => s.Conformities)
                .WithOne(a => a.Supplier)
                .OnDelete(DeleteBehavior.Restrict);
            //ne moje da se iztrie Supplier, predi
            //da se iztriqt vsichki Conformities, koito sa s tozi Supplier

            //da setna da se nulira zapisa na SupplierID v Article pri del 
            //na Supplier - TODO! 

            //mislq, che towa ne mi trqbwa weche, zashtoto gi vyrzah many-to-many
            //modelBuilder.Entity<Supplier>()
            //    .HasMany(s => s.Articles)
            //    .WithOne(a => a.Supplier)
            //    .OnDelete(DeleteBehavior.Restrict);//ne moje da se iztrie Supplier, predi
            //da se iztriqt vsichki Articles, koito sa s tozi Supplier!
        }
    }
}
