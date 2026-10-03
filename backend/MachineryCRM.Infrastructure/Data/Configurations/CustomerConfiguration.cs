using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(c => c.FiscalEntities).WithOne().HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Sites).WithOne().HasForeignKey(s => s.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Contacts).WithOne().HasForeignKey(c => c.CustomerId).OnDelete(DeleteBehavior.Cascade);
    }
}