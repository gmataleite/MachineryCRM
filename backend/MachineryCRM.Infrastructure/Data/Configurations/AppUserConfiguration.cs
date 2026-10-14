using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(150);
        builder.Property(a => a.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(a => a.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();
        builder.Property(a => a.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(a => a.MustChangePassword).IsRequired().HasDefaultValue(true);

        builder.HasIndex(a => a.Email).IsUnique();
    }
}