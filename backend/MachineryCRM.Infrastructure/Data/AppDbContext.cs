using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MachineryCRM.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<FiscalEntity> FiscalEntities => Set<FiscalEntity>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<GeoPoint> GeoPoints => Set<GeoPoint>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<TransferHistory> TransferHistories => Set<TransferHistory>();
    public DbSet<Maintenance> Maintenances => Set<Maintenance>();
    public DbSet<Communication> Communications => Set<Communication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Enables PostGIS and pgvector extensions as defined in the architectural requirements
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("vector");

        // Automatically applies all IEntityTypeConfiguration classes found in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Ignora o aviso estrutural de Owned Types opcionais sem chaves identificadoras
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.OptionalDependentWithoutIdentifyingPropertyWarning));
    }
}