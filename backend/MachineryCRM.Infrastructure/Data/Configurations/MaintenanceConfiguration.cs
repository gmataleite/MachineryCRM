using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
{
    public void Configure(EntityTypeBuilder<Maintenance> builder)
    {
        builder.ToTable("Maintenances");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.MaintenanceDate).IsRequired();
        builder.Property(m => m.MaintenanceType).IsRequired().HasMaxLength(100);
        builder.Property(m => m.PartsUsed).HasMaxLength(1000);
        builder.Property(m => m.Status).IsRequired().HasMaxLength(50);

        builder.HasOne<AppUser>()
               .WithMany()
               .HasForeignKey(a => a.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}