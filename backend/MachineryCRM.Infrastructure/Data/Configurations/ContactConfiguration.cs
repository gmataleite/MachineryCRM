using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.Observations).HasMaxLength(1000);

        builder.HasOne<Site>()
               .WithMany(s => s.Contacts)
               .HasForeignKey(c => c.SiteId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FiscalEntity>()
               .WithMany(f => f.Contacts)
               .HasForeignKey(c => c.FiscalEntityId)
               .OnDelete(DeleteBehavior.Cascade);

        // Comunicações associadas ao Contato
        builder.HasMany<Communication>()
               .WithOne()
               .HasForeignKey(c => c.ContactId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}