using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class FiscalEntityConfiguration : IEntityTypeConfiguration<FiscalEntity>
{
    public void Configure(EntityTypeBuilder<FiscalEntity> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Name).IsRequired().HasMaxLength(200);
        builder.Property(f => f.Country).IsRequired().HasMaxLength(100);
        builder.Property(f => f.State).IsRequired().HasMaxLength(100);
        builder.Property(f => f.City).IsRequired().HasMaxLength(100);

        builder.HasMany(f => f.Contacts).WithOne(c => c.FiscalEntity).HasForeignKey(c => c.FiscalEntityId).OnDelete(DeleteBehavior.SetNull);
    }
}