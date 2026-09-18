using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Country).IsRequired().HasMaxLength(100);
        builder.Property(s => s.State).IsRequired().HasMaxLength(100);
        builder.Property(s => s.City).IsRequired().HasMaxLength(100);

        builder.HasMany(s => s.GeoPoints).WithOne(g => g.Site).HasForeignKey(g => g.SiteId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.Contacts).WithOne(c => c.Site).HasForeignKey(c => c.SiteId).OnDelete(DeleteBehavior.SetNull);
    }
}