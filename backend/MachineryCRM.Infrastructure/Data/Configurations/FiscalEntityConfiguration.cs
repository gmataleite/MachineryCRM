using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class FiscalEntityConfiguration : IEntityTypeConfiguration<FiscalEntity>
{
    public void Configure(EntityTypeBuilder<FiscalEntity> builder)
    {
        builder.ToTable("FiscalEntities");
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Name).IsRequired().HasMaxLength(200);

        // Conversão do Value Object TaxId
        builder.Property(f => f.TaxId)
            .HasConversion(
                taxId => taxId != null ? taxId.Value : null,
                value => value != null ? new TaxId(value, "BR") : null) // BR hardcoded por inferência estrutural; avaliar injeção via factory se o domínio for global
            .HasColumnName("TaxId")
            .HasMaxLength(50);

        // Mapeamento do Value Object Address (Billing)
        builder.OwnsOne(f => f.BillingAddress, a =>
        {
            a.Property(p => p.AddressLine).HasColumnName("BillingAddressLine").HasMaxLength(200);
            a.Property(p => p.Neighborhood).HasColumnName("BillingNeighborhood").HasMaxLength(100);
            a.Property(p => p.City).HasColumnName("BillingCity").HasMaxLength(100);
            a.Property(p => p.State).HasColumnName("BillingState").HasMaxLength(50);
            a.Property(p => p.PostalCode).HasColumnName("BillingPostalCode").HasMaxLength(20);
            a.Property(p => p.CountryCode).HasColumnName("BillingCountryCode").HasMaxLength(10);
        });

        // Mapeamento do Value Object Address (Shipping)
        builder.OwnsOne(f => f.ShippingAddress, a =>
        {
            a.Property(p => p.AddressLine).HasColumnName("ShippingAddressLine").HasMaxLength(200);
            a.Property(p => p.Neighborhood).HasColumnName("ShippingNeighborhood").HasMaxLength(100);
            a.Property(p => p.City).HasColumnName("ShippingCity").HasMaxLength(100);
            a.Property(p => p.State).HasColumnName("ShippingState").HasMaxLength(50);
            a.Property(p => p.PostalCode).HasColumnName("ShippingPostalCode").HasMaxLength(20);
            a.Property(p => p.CountryCode).HasColumnName("ShippingCountryCode").HasMaxLength(10);
        });

        builder.HasMany(f => f.Contacts)
            .WithOne()
            .HasForeignKey(c => c.FiscalEntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}