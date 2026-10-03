using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder.ToTable("Machines");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.SerialNumber).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Model).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Brand).HasMaxLength(100);
        builder.Property(m => m.Status).HasMaxLength(50);
        builder.Property(m => m.AcquisitionDate).HasColumnType("date");

        builder.HasIndex(m => m.SerialNumber).IsUnique();

        builder.HasMany(m => m.TransferHistories)
            .WithOne()
            .HasForeignKey(t => t.MachineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Maintenances)
            .WithOne()
            .HasForeignKey(m => m.MachineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Communications)
            .WithOne()
            .HasForeignKey(c => c.MachineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}