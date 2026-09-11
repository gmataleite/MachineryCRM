using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.SerialNumber).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Model).IsRequired().HasMaxLength(150);
        // builder.Property(m => m.Manufacturer).IsRequired().HasMaxLength(150);
        
        builder.HasIndex(m => m.SerialNumber).IsUnique();
    }
}