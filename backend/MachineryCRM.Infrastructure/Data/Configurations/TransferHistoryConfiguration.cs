using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryCRM.Infrastructure.Data.Configurations;

public class TransferHistoryConfiguration : IEntityTypeConfiguration<TransferHistory>
{
    public void Configure(EntityTypeBuilder<TransferHistory> builder)
    {
        builder.ToTable("TransferHistories");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TransferDate).IsRequired();
        builder.Property(t => t.Reason).HasMaxLength(500);
        builder.Property(t => t.LoggedBy).HasMaxLength(150);

        builder.HasOne<Site>()
               .WithMany()
               .HasForeignKey(t => t.OriginSiteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Site>()
               .WithMany()
               .HasForeignKey(t => t.DestinationSiteId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}