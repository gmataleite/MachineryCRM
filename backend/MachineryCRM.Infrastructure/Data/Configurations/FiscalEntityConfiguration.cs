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
        builder.Property(f => f.TaxId).HasMaxLength(50);
        builder.Property(f => f.BillingAddress).HasMaxLength(400);
        builder.Property(f => f.ShippingAddress).HasMaxLength(400);

        builder.HasMany(f => f.Contacts).WithOne(c => c.FiscalEntity).HasForeignKey(c => c.FiscalEntityId).OnDelete(DeleteBehavior.SetNull);
    }
}