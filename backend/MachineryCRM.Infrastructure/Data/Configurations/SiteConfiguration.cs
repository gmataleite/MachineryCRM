using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("Sites");
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);

        builder.OwnsOne(s => s.Address, a =>
        {
            a.Property(p => p.AddressLine).HasColumnName("AddressLine").HasMaxLength(200);
            a.Property(p => p.Neighborhood).HasColumnName("Neighborhood").HasMaxLength(100);
            a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
            a.Property(p => p.State).HasColumnName("State").HasMaxLength(50);
            a.Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            a.Property(p => p.CountryCode).HasColumnName("CountryCode").HasMaxLength(10);
        });

        builder.HasMany(s => s.GeoPoints)
            .WithOne()
            .HasForeignKey(g => g.SiteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Contacts)
            .WithOne()
            .HasForeignKey(c => c.SiteId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(s => s.Machines)
            .WithOne()
            .HasForeignKey(m => m.SiteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}