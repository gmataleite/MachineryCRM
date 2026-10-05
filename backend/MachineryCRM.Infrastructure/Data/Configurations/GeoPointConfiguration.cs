using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class GeoPointConfiguration : IEntityTypeConfiguration<GeoPoint>
{
    public void Configure(EntityTypeBuilder<GeoPoint> builder)
    {
        builder.ToTable("GeoPoints");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Description).IsRequired().HasMaxLength(250);
        builder.Property(g => g.Latitude).HasPrecision(10, 7);
        builder.Property(g => g.Longitude).HasPrecision(10, 7);
        builder.Property(g => g.Order).IsRequired();
        
        // Mapeamento do Enum como string para maior clareza no banco
        builder.Property(g => g.LocationType)
               .HasConversion<string>()
               .HasMaxLength(50);
    }
}