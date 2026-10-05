using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class CommunicationConfiguration : IEntityTypeConfiguration<Communication>
{
    public void Configure(EntityTypeBuilder<Communication> builder)
    {
        builder.ToTable("Communications");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.InteractionDate).IsRequired();
        builder.Property(c => c.Channel).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Summary).IsRequired().HasMaxLength(2000);

        builder.HasOne<AppUser>()
               .WithMany()
               .HasForeignKey(a => a.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}